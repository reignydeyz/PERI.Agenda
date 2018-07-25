import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { MemberModule, Member } from '../member/member.component';
import { AccountModule } from '../account/account.component';
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html'
})
export class ProfileComponent {
    @Input('member') member: Member;

    private ac: AccountModule;
    private mm: MemberModule;
    private ex: ErrorExceptionModule;

    leading: number = 0;
    following: number = 0;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ex = new ErrorExceptionModule();
        this.ex.baseUrl = this.baseUrl;

        this.mm = new MemberModule();
        this.mm.http = http;
        this.mm.baseUrl = baseUrl;

        this.ac = new AccountModule();
        this.ac.baseUrl = baseUrl;
        this.ac.http = http;
    }

    ngOnInit() {
        if (this.member == null || this.member == undefined) {
            this.ac.getProfile()
                .subscribe(r => {
                    this.member = r;
                    this.titleService.setTitle('Profile');
                });
        }
    }
}