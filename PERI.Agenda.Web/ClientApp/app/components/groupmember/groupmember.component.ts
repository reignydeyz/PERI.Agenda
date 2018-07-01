import { Component, Input, Inject, OnChanges } from '@angular/core';
import * as $ from "jquery";

import { Group, GroupComponent } from '../group/group.component';
import { Http } from '@angular/http';
import { Title } from '@angular/platform-browser';
import { Member } from '../member/member.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

@Component({
    selector: 'groupmember',
    templateUrl: './groupmember.component.html',
    styleUrls: ['../table/table.component.css']
})
export class GroupMemberComponent {
    @Input() group: Group;
    members: Member[];

    private ex: ErrorExceptionModule;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ex = new ErrorExceptionModule();
        this.ex.baseUrl = baseUrl;
    }

    ngOnChanges() {
        this.http.post(this.baseUrl + 'api/groupmember/find/' + this.group.id, new Member()).subscribe(result => {
            this.members = result.json();
        }, error => this.ex.catchError(error));

        if (this.members) {
            var tbl = <HTMLTableElement>document.getElementById("tblModalMembers");
            if (tbl != null && tbl != undefined) {
                let tbl1: any;
                tbl1 = $("#tblModalMembers");
                tbl.onscroll = function () {
                    $("#tblModalMembers > *").width(tbl1.width() + tbl1.scrollLeft());
                };
            }
        }
    }

    public onChecklistLoad(groupId: number) {

        // Add code here
    }
}
