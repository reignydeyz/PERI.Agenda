import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm } from '@angular/forms';
import * as $ from "jquery";

@Component({
    selector: 'member',
    templateUrl: './member.component.html'
})

export class MemberComponent {
    public member: Member;
    public members: Member[];

    _http: Http;
    _baseUrl: string;

    private find(m: Member) {
        this._http.post(this._baseUrl + 'api/member/find', {
            name: m.name,
            email: m.email
        }).subscribe(result => {
            this.members = result.json() as Member[];
        }, error => console.error(error));
    }

    private add(m: Member) {
        this._http.post(this._baseUrl + 'api/member/new', {
            name: m.name,
            email: m.email
        }).subscribe(result => { alert('Added!'); $('#modalNew').modal('toggle'); }, error => console.error(error));
    }

    private edit(m: Member) {
        this._http.post(this._baseUrl + 'api/member/edit', {
            name: m.name,
            email: m.email
        }).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => console.error(error));
    }

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;
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
        m.email = f.controls['email'].value;

        this.add(m);
    }

    public onEditInit(id: number) {
        this._http.get(this._baseUrl + 'api/member/findbyid?id=' + id)
            .subscribe(result => { this.member = result.json() as Member }, error => console.error(error));
    }

    public onEditSubmit(f: NgForm) {
        var m = new Member();
        m.name = f.controls['name'].value;
        m.email = f.controls['email'].value;

        this.edit(m);
    }
}

class Member {
    name: string;
    nickName: string;
    email: string;
    address: string;
    mobile: string;
}
