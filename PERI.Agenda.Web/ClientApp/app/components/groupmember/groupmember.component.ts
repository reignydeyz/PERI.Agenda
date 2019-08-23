import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { GroupComponent } from '../group/group.component';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Title } from '@angular/platform-browser';
import { Pager } from '../pager/pager.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { NgForm } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { saveAs } from 'file-saver';
import { Member } from '../../models/member';
import { Group } from '../../models/group';
import { GroupMember } from '../../models/groupmember';
import { GroupMemberService } from '../../services/groupmember.service';

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

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private gmm: GroupMemberService, private ex: ErrorExceptionModule) {
        
    }

    private async paginate(member: string, page: number) {
        this.chunk = await this.gmm.search(this.group.id, member, page);
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    private async download(groupId: number) {
        this.downloadFile(await this.download(groupId));
    }

    ngOnInit() {
        this.search = "";
        this.pager = new Pager();
    }

    async ngOnChanges() {
        this.members = await this.gmm.find(this.group.id, new Member());

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

    async toggle(gm: GroupMember) {
        if (gm.groupId > 0) {
            await this.gmm.delete(gm).then(() => {
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
            }).catch(error => {
                this.ex.catchError(error);
            });
        }
        else {
            gm.groupId = this.group.id;

            await this.gmm.add(gm).then(() => {
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

class Chunk {
    checklist: GroupMember[];
    pager: Pager;
}