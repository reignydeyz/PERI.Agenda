import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import * as $ from "jquery";

import { LookUpModule, LookUp } from "../lookup/lookup.component";
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { IMyDpOptions } from 'mydatepicker';
import * as moment from 'moment';
import { Title } from '@angular/platform-browser';
import { Observable } from 'rxjs/Observable';
import { NgForm } from '@angular/forms';
import { Role } from '../../models/role';
import { Member } from '../../models/member';
import { AccountService } from '../../services/account.service';
import { MemberService } from '../../services/member.service';

@Component({
    selector: 'account',
    templateUrl: './account.component.html'
})
export class AccountComponent {
    public genders: LookUp[];
    public profile: Member;
    private ex: ErrorExceptionModule;

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
        private am: AccountService,
        private mm: MemberService
    ) {

        this.profile = new Member();

        this.ex = new ErrorExceptionModule();
        this.ex.baseUrl = this.baseUrl;
    }

    async ngAfterViewInit() {
        var lm = new LookUpModule();
        lm.http = this.http;
        lm.baseUrl = this.baseUrl;
        lm.getByGroup('Gender').subscribe(result => { this.genders = result });
        
        this.profile = await this.am.getProfile();
        this.titleService.setTitle(this.profile.name);

        if (moment(this.profile.birthDate).isValid() == true) {
            this.profile.birthDate = { date: { year: moment(this.profile.birthDate).format('YYYY'), month: moment(this.profile.birthDate).format('M'), day: moment(this.profile.birthDate).format('D') } };
        }
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

    async onEditSubmit(m: Member) {
        if (m.birthDate != null) {
            m.birthDate = moment(m.birthDate.date.month + '/' + m.birthDate.date.day + '/' + m.birthDate.date.year).format('MM/DD/YYYY');
        }

        m.isActive = true;
        await this.mm.edit(m);
    }

    public onDeactivateSubmit(f: NgForm) {
        this.http.post(this.baseUrl + 'api/account/deactivate', {
            password: f.controls['currentPassword'].value
        }).subscribe(result => {
            alert('Deactivated!');
            window.location.replace(this.baseUrl + 'authentication/signout');
        }, error => this.ex.catchError(error));
    }
}