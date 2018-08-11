import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { MemberModule, Member } from '../member/member.component';
import { AccountModule } from '../account/account.component';
import { Title } from '@angular/platform-browser';
import * as $ from "jquery";

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css',
        '../table/table.component.css'
    ]
})
export class ProfileComponent {
    @Input('member') member: Member;

    private ac: AccountModule;
    private mm: MemberModule;
    private ex: ErrorExceptionModule;

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

                    this.mm.leading(r.id).subscribe(res => {
                        this.member.leading = res
                    });

                    this.mm.following(r.id).subscribe(res => {
                        this.member.following = res
                    });

                    this.mm.invites(r.id).subscribe(res => {
                        this.member.invites = res
                    });

                    this.mm.activities(r.id).subscribe(res => {
                        this.member.activities = res
                    });

                    this.titleService.setTitle(r.name);
                });
        }
    }

    ngAfterViewChecked() {
        if (this.member && this.member.activities) {
            var tbl = <HTMLTableElement>document.getElementById("tblProfile");
            if (tbl != null && tbl != undefined) {
                let tbl1: any;
                tbl1 = $("#tblProfile");
                tbl.onscroll = function () {
                    $("#tblProfile > *").width(tbl1.width() + tbl1.scrollLeft());
                };
            }
        }
    }
}