import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { LookUpModule, LookUp } from "../lookup/lookup.component";
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Pager } from '../pager/pager.component';

import * as moment from 'moment';
import { Title } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';

import { saveAs } from 'file-saver';
import { IMyDpOptions } from 'mydatepicker';
import { Role } from '../../models/role';
import { Member } from '../../models/member';
import { Activity } from '../../models/activity';
import { MemberService } from '../../services/member.service';
import { RoleService } from '../../services/role.service';

@Component({
    selector: 'member',
    templateUrl: './member.component.html',
    styleUrls: ['./member.component.css',
        '../table/table.component.css'
    ]
})
export class MemberComponent {
    public total: number;
    public actives: number;
    public inactives: number;
    public member: Member = new Member();
    public genders: LookUp[];
    public statuses: boolean[];
    public roles: Role[];

    public search: Member;
    public pager: Pager;
    public chunk: Chunk;

    public enableEmail: boolean = true;

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };
    
    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    async paginate(m: Member, page: number) {
        this.chunk = await this.mm.find(m, page) as Chunk;
    }

    async download(m: Member) {
        this.downloadFile(await this.mm.download(m));
    }

    async getTotal() {
        this.total = await this.mm.getTotal("all");
        this.actives = await this.mm.getTotal("active");
        this.inactives = await this.mm.getTotal("inactive");
    }

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private mm: MemberService, private roleService: RoleService, private ex: ErrorExceptionModule) {
        
    }

    async ngOnInit() {
        this.member = new Member();
        this.search = new Member();
        this.pager = new Pager();
        this.paginate(this.member, 1);

        this.statuses = [false, true];

        await this.getTotal();

        this.titleService.setTitle('Members');

        // Populate roles
        this.roles = await this.roleService.getAll();
    }

    // https://www.concretepage.com/angular-2/angular-2-ngform-with-ngmodel-directive-example
    public onSearchSubmit(f: NgForm) {
        var m = new Member();
        m.name = f.controls['name'].value;
        m.email = f.controls['email'].value;
        m.roleId = f.controls['roleId'].value;
        m.isActive = f.controls['isActive'].value;

        this.paginate(m, 1);
        this.search = m;
    }

    public onPaginate(page: number) {        
        this.paginate(this.search, page);
    }

    public onDownloadClick() {
        this.download(this.search);
    }

    async onNewSubmit(f: NgForm) {
        var middleInitial = (f.controls['middleInitial'].value == ''
            || f.controls['middleInitial'].value == null
            || f.controls['middleInitial'].value == undefined) ? ' ' : ' ' + f.controls['middleInitial'].value + '. ';
        var name = f.controls['firstName'].value.trim() + middleInitial + f.controls['lastName'].value.trim();

        var m = new Member();
        m.name = name;
        m.nickName = f.controls['nickName'].value;

        if (f.controls['birthDate'].value != null) {
            m.birthDate = f.controls['birthDate'].value.formatted;
        }
        else {
            m.birthDate = '';
        }
        
        m.email = f.controls['email'].value;
        m.address = f.controls['address'].value;
        m.mobile = f.controls['mobile'].value;
        m.gender = f.controls['gender'].value;
        m.invitedByMemberName = f.controls['invitedBy'].value;
        m.remarks = f.controls['remarks'].value;

        await this.mm.add(m).then(r => {
            m.id = r;

            this.chunk.members.splice(0, 0, m);
            this.chunk.pager.totalItems++;

            this.actives += 1;
            this.total += 1;

            alert('Added!');
            $('#modalNew').modal('toggle');
        }, err => {
            this.ex.catchError(err);
        });
    }

    async onEditInit(id: number) {
        this.member = await this.mm.get(id);
        this.enableEmail = !this.member.email || this.member.email.length == 0;
        if (moment(this.member.birthDate).isValid() == true) {
            this.member.birthDate = { date: { year: moment(this.member.birthDate).format('YYYY'), month: moment(this.member.birthDate).format('M'), day: moment(this.member.birthDate).format('D') } };
        }
    }

    async onEditSubmit(member: Member) {
        if (this.member.birthDate != null) {
            this.member.birthDate = moment(this.member.birthDate.date.month + '/' + this.member.birthDate.date.day + '/' + this.member.birthDate.date.year).format('MM/DD/YYYY');
        }

        await this.mm.edit(member).then(r => {
            for (let m of this.chunk.members) {
                if (m.id == member.id) {
                    let index: number = this.chunk.members.indexOf(m);
                    this.chunk.members[index] = member;
                }
            }

            if (member.isActive) {
                this.actives += 1;
                this.inactives -= 1;
            }
            else {
                this.actives -= 1;
                this.inactives += 1;
            }

            alert('Updated!');
            $('#modalEdit').modal('toggle');
        }, err => {
            this.ex.catchError(err);
        });  
    }

    // https://stackoverflow.com/questions/20043265/check-if-checkbox-element-is-checked-in-typescript
    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () { 
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    // https://stackoverflow.com/questions/34547127/angular2-equivalent-of-document-ready
    async ngAfterViewInit() {
        var lm = new LookUpModule();
        lm.http = this.http;
        lm.baseUrl = this.baseUrl;
        lm.getByGroup('Gender').subscribe(result => { this.genders = result });

        this.names = await this.mm.allNames();        
    }

    ngAfterViewChecked() {
        if (this.chunk) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("table");
            tbl.onscroll = function () {
                $("table > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }
    }

    async onDeleteClick() {
        var flag = confirm('Are you sure you want to delete selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        await this.mm.delete(selectedIds).then(() => {
            for (let id of selectedIds) {
                for (let m of this.chunk.members) {
                    if (m.id == id) {

                        if (m.isActive) {
                            this.actives -= 1;
                        }
                        else {
                            this.inactives -= 1;
                        }

                        this.chunk.members.splice(this.chunk.members.indexOf(m), 1);

                        this.chunk.pager.totalItems--;
                    }
                }
            }

            this.total -= selectedIds.length;

            alert('Success!');
        }).catch(error => {
            this.ex.catchError(error);
        });
    }

    async onActivateClick() {
        var flag = confirm('Are you sure you want to activate selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        await this.mm.activate(selectedIds).then(() => {
            for (let id of selectedIds) {
                for (let m of this.chunk.members) {
                    if (m.id == id) {
                        this.chunk.members[this.chunk.members.indexOf(m)].isActive = true;
                    }
                }
            }

            this.actives += selectedIds.length;
            this.inactives -= selectedIds.length;

            alert('Success!');
        }).catch(error => {
            this.ex.catchError(error);
        });
    }

    async onDeactivateClick() {
        var flag = confirm('Are you sure you want to deactivate selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        await this.mm.deactivate(selectedIds).then(() => {
            for (let id of selectedIds) {
                for (let m of this.chunk.members) {
                    if (m.id == id) {
                        this.chunk.members[this.chunk.members.indexOf(m)].isActive = false;
                    }
                }
            }

            this.actives -= selectedIds.length;
            this.inactives += selectedIds.length;

            alert('Success!');
        }).catch(error => {
            this.ex.catchError(error);
        });
    }

    async onModalProfileInit(id: number) {
        this.member = await this.mm.get(id);
        this.member.leading = await this.mm.leading(id);
        this.member.following = await this.mm.following(id);
        this.member.invites = await this.mm.invites(id);
        this.member.activities = await this.mm.activities(id);        
    }

    public names: string[] = [];
    suggestionsForNew: string[] = [];
    suggestionsForEdit: string[] = [];

    // New
    suggestForNew(fc: any) {
        if (fc.value.length > 0) {
            this.suggestionsForNew = this.names
                .filter(c => c.startsWith(fc.value.toUpperCase()))
                .slice(0, 5);
        }
        else {
            this.suggestionsForNew.length = 0;
        }
    }

    suggestionForNewSelect(f: NgForm, s: string) {
        this.suggestionsForNew.length = 0;
        
        f.controls['invitedBy'].setValue(s);
    }

    // Edit
    suggestForEdit(fc: any) {
        if (fc.value.length > 0) {
            this.suggestionsForEdit = this.names
                .filter(c => c.startsWith(fc.value.toUpperCase()))
                .slice(0, 5);
        }
        else {
            this.suggestionsForEdit.length = 0;
        }
    }

    suggestionForEditSelect(f: NgForm, s: string) {
        this.suggestionsForEdit.length = 0;

        this.member.invitedByMemberName = s;
    }

    async onRoleChange(member: Member, roleId: number) {
        var flag = confirm('This will change the user`s role. Continue?');

        if (!flag)
            return false;

        member.roleId = roleId;

        await this.mm.updateRole(member).then(() => {
            for (let m of this.chunk.members) {
                if (m.id == member.id) {
                    let index: number = this.chunk.members.indexOf(m);
                    this.chunk.members[index] = member;
                }
            }
        });

        return false;
    }
}

class Chunk {
    members: Member[];
    pager: Pager;
}