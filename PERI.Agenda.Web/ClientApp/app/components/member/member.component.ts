import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { LookUpComponent, LookUp } from "../lookup/lookup.component";
import * as moment from 'moment';

@Component({
    selector: 'member',
    templateUrl: './member.component.html',
    styleUrls: ['./member.component.css']
})

export class MemberComponent {
    public total: number;
    public actives: number;
    public inactives: number;
    public member: Member;
    public members: Member[];
    public genders: LookUp[];
    public statuses: boolean[];

    _http: Http;
    _baseUrl: string;

    private find(m: Member) {
        this._http.post(this._baseUrl + 'api/member/find', {
            name: m.name,
            nickName: m.nickName,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile
        }).subscribe(result => {
            this.members = result.json() as Member[];
        }, error => console.error(error));
    }

    private add(m: Member) {
        this._http.post(this._baseUrl + 'api/member/new', {
            name: m.name,
            nickName: m.nickName,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
            isActive: m.isActive
        }).subscribe(result => { alert('Added!'); $('#modalNew').modal('toggle'); }, error => { console.error(error); alert('Oops! Unknown error has occured.') });
    }

    private edit(m: Member) {
        this._http.post(this._baseUrl + 'api/member/edit', {
            id: m.id,
            name: m.name,
            nickName: m.nickName,
            gender: m.gender,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
            isActive: m.isActive
        }).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => { console.error(error); alert('Oops! Unknown error has occured.') });
    }

    private getTotal() {
        this._http.get(this._baseUrl + 'api/member/total').subscribe(result => {
            this.total = result.json() as number;
        }, error => console.error(error));

        this._http.get(this._baseUrl + 'api/member/total?q=active').subscribe(result => {
            this.actives = result.json() as number;
        }, error => console.error(error));

        this._http.get(this._baseUrl + 'api/member/total?q=inactive').subscribe(result => {
            this.inactives = result.json() as number;
        }, error => console.error(error));
    }

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;
        this.member = new Member();
        this.find(new Member());

        this.statuses = [false, true];

        this.getTotal();
    }

    // https://www.concretepage.com/angular-2/angular-2-ngform-with-ngmodel-directive-example
    public onSearchSubmit(f: NgForm) {
        var m = new Member();
        m.name = f.controls['name'].value;
        m.email = f.controls['email'].value;

        this.find(m);
    }

    public onNewSubmit(f: NgForm) {
        var m = new Member();
        m.name = f.controls['name'].value;
        m.nickName = f.controls['nickName'].value;
        m.birthDate = f.controls['birthDate'].value;
        m.email = f.controls['email'].value;
        m.address = f.controls['address'].value;
        m.mobile = f.controls['mobile'].value;

        this.add(m);

        this.members.push(m);

        this.actives += 1;
        this.total += 1;
    }

    public onEditInit(id: number) {
        this._http.get(this._baseUrl + 'api/member/findbyid?id=' + id)
            .subscribe(result => { this.member = result.json() as Member }, error => console.error(error));
    }

    public onEditSubmit(member: Member) {
        var date = <HTMLInputElement>document.getElementById("txtDate");
        this.member.birthDate = date.value;

        this.edit(member);  

        for (let m of this.members) {
            if (m.id == member.id) {
                let index: number = this.members.indexOf(m);
                this.members[index] = member;
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

        console.log(member);
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
        var lc = new LookUpComponent();
        lc.http = this._http;
        lc.baseUrl = this._baseUrl;
        lc.getByGroup('Gender').subscribe(result => { this.genders = result });
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

        let options : any = {};
        options.url = "api/member/delete";
        options.type = "POST";
        options.data = JSON.stringify(selectedIds);
        options.contentType = "application/json";
        options.dataType = "json";
        options.success = () => {

            for (let id of selectedIds) {
                for (let m of this.members) {
                    if (m.id == id) {

                        if (m.isActive) {
                            this.actives -= 1;
                        }
                        else {
                            this.inactives -= 1;
                        }

                        this.members.splice(this.members.indexOf(m), 1);
                    }
                }
            }

            this.total -= selectedIds.length;            

            alert('Success!');
        };
        options.error = function () {
            alert("Error while deleting the records!");
        };
        $.ajax(options);
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

        let options: any = {};
        options.url = "api/member/activate";
        options.type = "POST";
        options.data = JSON.stringify(selectedIds);
        options.contentType = "application/json";
        options.dataType = "json";
        options.success = () => {

            for (let id of selectedIds) {
                for (let m of this.members) {
                    if (m.id == id) {
                        this.members[this.members.indexOf(m)].isActive = true;
                    }
                }
            }

            this.actives += selectedIds.length;
            this.inactives -= selectedIds.length;

            alert('Success!');
        };
        options.error = function () {
            alert("Error while deleting the records!");
        };
        $.ajax(options);
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

        let options: any = {};
        options.url = "api/member/deactivate";
        options.type = "POST";
        options.data = JSON.stringify(selectedIds);
        options.contentType = "application/json";
        options.dataType = "json";
        options.success = () => {

            for (let id of selectedIds) {
                for (let m of this.members) {
                    if (m.id == id) {
                        this.members[this.members.indexOf(m)].isActive = false;
                    }
                }
            }

            this.actives -= selectedIds.length;
            this.inactives += selectedIds.length;

            alert('Success!');
        };
        options.error = function () {
            alert("Error while deleting the records!");
        };
        $.ajax(options);
    }
}

export class Member {
    id: number;
    name: string;
    nickName: string;
    birthDate: string;
    gender: number;
    email: string;
    address: string;
    mobile: string;
    isActive: boolean;
}
