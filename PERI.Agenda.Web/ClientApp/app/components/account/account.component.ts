import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import * as $ from "jquery";

import { Member, MemberModule } from '../member/member.component';
import { LookUpModule, LookUp } from "../lookup/lookup.component";
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { IMyDpOptions } from 'mydatepicker';
import * as moment from 'moment';
import { Title } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';
import { NgForm } from '@angular/forms';

export class AccountModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public getProfile() : Observable<Member> {
        return this.http.get(this.baseUrl + 'api/account/profile')
            .map(r => r.json());
    }

    public getRole(): Observable<Role> {
        return this.http.get(this.baseUrl + 'api/account/role')
            .map(r => r.json());
    }
}

@Component({
    selector: 'account',
    templateUrl: './account.component.html'
})
export class AccountComponent {
    public genders: LookUp[];
    public profile: Member;

    private am: AccountModule;
    private mm: MemberModule;
    private ex: ErrorExceptionModule;

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.am = new AccountModule();
        this.am.http = http;
        this.am.baseUrl = baseUrl;

        this.mm = new MemberModule();
        this.mm.http = http;
        this.mm.baseUrl = baseUrl;

        this.profile = new Member();

        this.ex = new ErrorExceptionModule();
        this.ex.baseUrl = this.baseUrl;
    }

    ngAfterViewInit() {
        var lm = new LookUpModule();
        lm.http = this.http;
        lm.baseUrl = this.baseUrl;
        lm.getByGroup('Gender').subscribe(result => { this.genders = result });

        this.am.getProfile()
            .subscribe(r => {
                this.profile = r;
                this.titleService.setTitle(r.name);

                if (moment(this.profile.birthDate).isValid() == true) {
                    this.profile.birthDate = { date: { year: moment(this.profile.birthDate).format('YYYY'), month: moment(this.profile.birthDate).format('M'), day: moment(this.profile.birthDate).format('D') } };
                }
            });
    }

    public onChangePasswordSubmit(f: NgForm) {
        this.http.post(this.baseUrl + 'api/account/changepassword', {
            currentPassword: f.controls['currentPassword'].value,
            newPassword: f.controls['newPassword'].value,
            reEnterNewPassword: f.controls['reNewPassword'].value
        }).subscribe(result => {
            alert('Updated!');
            window.location.replace(this.baseUrl + 'authentication/signout');
        }, error => this.ex.catchError(error));
    }

    public onEditSubmit(m: Member) {
        if (m.birthDate != null) {
            m.birthDate = moment(m.birthDate.date.month + '/' + m.birthDate.date.day + '/' + m.birthDate.date.year).format('MM/DD/YYYY');
        }

        this.mm.edit(m);
    }
}

export class Role {
    roleId: string;
    name: string;
}