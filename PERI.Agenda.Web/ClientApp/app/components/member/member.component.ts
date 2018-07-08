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

export class MemberModule {
    public http: Http;
    public baseUrl: string;
    private ex: ErrorExceptionModule;

    public add(m: Member): Observable<number> {
        return this.http.post(this.baseUrl + 'api/member/new', {
            name: m.name,
            nickName: m.nickName,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
            isActive: m.isActive,
            gender: m.gender
        }).map(response => response.json());
    }

    public allNames(): Observable<string[]> {
        return this.http.get(this.baseUrl + 'api/member/allnames').map(response => response.json());
    }

    public edit(m: Member) {
        this.ex = new ErrorExceptionModule();

        this.http.post(this.baseUrl + 'api/member/edit', {
            id: m.id,
            name: m.name,
            nickName: m.nickName,
            gender: m.gender,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
            isActive: m.isActive
        }).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => this.ex.catchError(error));
    }
}

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
    public member: Member;
    public genders: LookUp[];
    public statuses: boolean[];

    public search: Member;
    public pager: Pager;
    public chunk: Chunk;

    private mm: MemberModule;

    private ex: ErrorExceptionModule;

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };
    
    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    private paginate(m: Member, page: number) {
        this.http.post(this.baseUrl + 'api/member/find/page/' + page, m).subscribe(result => {
            this.chunk = result.json() as Chunk;
        }, error => this.ex.catchError(error));
    }

    private download(m: Member) {
        this.http.post(this.baseUrl + 'api/member/download', m).subscribe(result => {
            let parsedResponse = result.text();
            this.downloadFile(parsedResponse);
        }, error => this.ex.catchError(error));;
    }

    private getTotal() {
        this.http.get(this.baseUrl + 'api/member/total/all').subscribe(result => {
            this.total = result.json() as number;
        }, error => this.ex.catchError(error));

        this.http.get(this.baseUrl + 'api/member/total/active').subscribe(result => {
            this.actives = result.json() as number;
        }, error => this.ex.catchError(error));

        this.http.get(this.baseUrl + 'api/member/total/inactive').subscribe(result => {
            this.inactives = result.json() as number;
        }, error => this.ex.catchError(error));
    }

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ex = new ErrorExceptionModule();
        this.ex.baseUrl = this.baseUrl;

        this.mm = new MemberModule();
        this.mm.http = http;
        this.mm.baseUrl = baseUrl;
    }

    ngOnInit() {
        this.member = new Member();
        this.search = new Member();
        this.pager = new Pager();
        this.paginate(this.member, 1);

        this.statuses = [false, true];

        this.getTotal();

        this.titleService.setTitle('Members');
    }

    // https://www.concretepage.com/angular-2/angular-2-ngform-with-ngmodel-directive-example
    public onSearchSubmit(f: NgForm) {
        var m = new Member();
        m.name = f.controls['name'].value;
        m.email = f.controls['email'].value;

        this.paginate(m, 1);
        this.search = m;
    }

    public onPaginate(page: number) {        
        this.paginate(this.search, page);
    }

    public onDownloadClick() {
        this.download(this.search);
    }

    public onNewSubmit(f: NgForm) {
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

        this.mm.add(m).subscribe(
            result => {
                m.id = result;
                this.chunk.members.push(m);

                this.actives += 1;
                this.total += 1;

                alert('Added!');
                $('#modalNew').modal('toggle');
            },
            error => this.ex.catchError(error));
    }

    public onEditInit(id: number) {
        this.http.get(this.baseUrl + 'api/member/get/' + id)
            .subscribe(result => {
                this.member = result.json() as Member;

                if (moment(this.member.birthDate).isValid() == true) {
                    this.member.birthDate = { date: { year: moment(this.member.birthDate).format('YYYY'), month: moment(this.member.birthDate).format('M'), day: moment(this.member.birthDate).format('D') } };
                }
            }, error => this.ex.catchError(error));
    }

    public onEditSubmit(member: Member) {
        if (this.member.birthDate != null) {
            this.member.birthDate = moment(this.member.birthDate.date.month + '/' + this.member.birthDate.date.day + '/' + this.member.birthDate.date.year).format('MM/DD/YYYY');
        }

        this.mm.edit(member);  

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
    ngAfterViewInit() {
        var lm = new LookUpModule();
        lm.http = this.http;
        lm.baseUrl = this.baseUrl;
        lm.getByGroup('Gender').subscribe(result => { this.genders = result });
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

    onDeleteClick() {
        var flag = confirm('Are you sure you want to delete selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/member/delete', body, options).subscribe(result => {

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
                    }
                }
            }

            this.total -= selectedIds.length;

            alert('Success!');

        }, error => this.ex.catchError(error));
    }

    onActivateClick() {
        var flag = confirm('Are you sure you want to activate selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/member/activate', body, options).subscribe(result => {

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

        }, error => this.ex.catchError(error));
    }

    onDeactivateClick() {
        var flag = confirm('Are you sure you want to deactivate selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/member/deactivate', body, options).subscribe(result => {

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

        }, error => this.ex.catchError(error));        
    }
}

export class Member {
    id: number;
    name: string;
    nickName: string;
    birthDate: any;
    gender: number;
    email: string;
    address: string;
    mobile: string;
    isActive: boolean;
}

class Chunk {
    members: Member[];
    pager: Pager;
}