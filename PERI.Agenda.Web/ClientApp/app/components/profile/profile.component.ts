import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Title } from '@angular/platform-browser';
import * as $ from "jquery";
import { Member } from '../../models/member';
import { MemberService } from '../../services/member.service';
import { AccountService } from '../../services/account.service';

@Component({
    selector: 'profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css',
        '../table/table.component.css'
    ]
})
export class ProfileComponent {
    @Input('member') member: Member;

    private ex: ErrorExceptionModule;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private mm: MemberService, private ac: AccountService) {
        this.ex = new ErrorExceptionModule();
        this.ex.baseUrl = this.baseUrl;
    }

    async ngOnInit() {
        if (this.member == null || this.member == undefined) {
            this.member = await this.ac.getProfile();
            this.member.leading = await this.mm.leading(this.member.id);
            this.member.following = await this.mm.following(this.member.id);
            this.member.invites = await this.mm.invites(this.member.id);
            this.member.activities = await this.mm.activities(this.member.id);

            this.titleService.setTitle(this.member.name);
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