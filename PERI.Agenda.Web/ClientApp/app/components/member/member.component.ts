import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { LookUpComponent, LookUp } from "../lookup/lookup.component";

@Component({
    selector: 'member',
    templateUrl: './member.component.html'
})

export class MemberComponent {
    public member: Member;
    public members: Member[];
    public genders: LookUp[];

    _http: Http;
    _baseUrl: string;

    private find(m: Member) {
        this._http.post(this._baseUrl + 'api/member/find', {
            name: m.name,
            nickName: m.nickName,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
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
        }).subscribe(result => { alert('Added!'); $('#modalNew').modal('toggle'); }, error => { console.error(error); alert('Oops! Unknown error has occured.') });
    }

    private edit(m: Member) {
        this._http.post(this._baseUrl + 'api/member/edit', {
            id: m.id,
            name: m.name,
            nickName: m.nickName,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
        }).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => { console.error(error); alert('Oops! Unknown error has occured.') });
    }

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;
        this.member = new Member();
        this.find(new Member());
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
    }

    public onEditInit(id: number) {
        this._http.get(this._baseUrl + 'api/member/findbyid?id=' + id)
            .subscribe(result => { this.member = result.json() as Member }, error => console.error(error));
    }

    public onEditSubmit(member: Member) {        
        this.edit(member);
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
}
