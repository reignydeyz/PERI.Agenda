import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { Group, GroupComponent } from '../group/group.component';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Title } from '@angular/platform-browser';
import { Member } from '../member/member.component';
import { Pager } from '../pager/pager.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { NgForm } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { saveAs } from 'file-saver';

export class GroupMemberModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public add(gm: GroupMember): Observable<number> {
        return this.http.put(this.baseUrl + 'api/groupmember/add', {
            memberId: gm.memberId,
            groupId: gm.groupId
        }).map(r => r.json());
    }

    public delete(gm: GroupMember) {
        return this.http.post(this.baseUrl + 'api/groupmember/delete', {
            memberId: gm.memberId,
            groupId: gm.groupId
        });
    }
}

@Component({
    selector: 'groupmember',
    templateUrl: './groupmember.component.html',
    styleUrls: ['./groupmember.component.css',
        '../table/table.component.css'
    ]
})
export class GroupMemberComponent {
    @Input() group: Group;
    @Output() change: EventEmitter<number> = new EventEmitter<number>();

    members: Member[];

    public search: string;
    public pager: Pager;
    public chunk: Chunk;

    public gmm: GroupMemberModule;

    private ex: ErrorExceptionModule;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ex = new ErrorExceptionModule();
        this.ex.baseUrl = baseUrl;

        this.gmm = new GroupMemberModule();
        this.gmm.http = http;
        this.gmm.baseUrl = baseUrl;
        this.gmm.ex = new ErrorExceptionModule();
    }

    private paginate(member: string, page: number) {
        let body = JSON.stringify(member);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/groupmember/' + this.group.id + '/checklist/page/' + page, body, options).subscribe(result => {
            this.chunk = result.json() as Chunk;
        }, error => this.ex.catchError(error));
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    private download(groupId: number) {
        this.http.get(this.baseUrl + 'api/groupmember/' + groupId + '/download').subscribe(result => {
            let parsedResponse = result.text();
            this.downloadFile(parsedResponse);
        }, error => this.gmm.ex.catchError(error));;
    }

    ngOnInit() {
        this.search = "";
        this.pager = new Pager();
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

    public onDownloadClick() {
        this.download(this.group.id);
    }

    public onChecklistSearchSubmit(f: NgForm) {
        this.paginate(f.controls['name'].value, 1);
        this.search = f.controls['name'].value;
    }

    public onChecklistLoad(groupId: number) {
        var frm = <HTMLFormElement>document.getElementById('frmSearchChecklist');
        frm.reset();

        this.search = "";

        this.paginate(this.search, 1);
    }

    public onPaginate(page: number) {
        this.paginate(this.search, page);
    }

    toggle(gm: GroupMember) {
        if (gm.groupId > 0) {
            this.gmm.delete(gm).subscribe(result => {
                for (let r of this.chunk.checklist) {
                    if (r.memberId == gm.memberId) {
                        let index: number = this.chunk.checklist.indexOf(r);

                        gm.groupId = 0;
                        this.chunk.checklist[index] = gm;

                        // Remove from members
                        for (let e of this.members) {
                            if (e.id == gm.memberId) {
                                this.members.splice(this.members.indexOf(e), 1);
                            }
                        }
                    }
                }

                this.change.emit(this.group.id);
                //alert('Success');
            }, error => this.ex.catchError(error));
        }
        else {
            gm.groupId = this.group.id;

            this.gmm.add(gm).subscribe(result => {
                for (let r of this.chunk.checklist) {
                    if (r.memberId == gm.memberId) {
                        let index: number = this.chunk.checklist.indexOf(r);
                        this.chunk.checklist[index] = gm;

                        // Add to members
                        var member = new Member();
                        member.id = gm.memberId;
                        member.name = gm.name;
                        this.members.push(member);
                    }
                }

                this.change.emit(this.group.id);
                //alert('Success');
            }, error => this.ex.catchError(error));
        }
    }
}

export class GroupMember {
    memberId: number;
    name: string;
    groupId: number;
}

class Chunk {
    checklist: GroupMember[];
    pager: Pager;
}