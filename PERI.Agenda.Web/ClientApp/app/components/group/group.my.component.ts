﻿import { Component, Inject, AfterViewInit, group } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel, FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import * as $ from "jquery";

import { Observable } from 'rxjs/Observable';

import { GroupCategoryModule } from '../groupcategory/groupcategory.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Pager } from '../pager/pager.component';
import { Title } from '@angular/platform-browser';
import { GroupMemberModule, GroupMember } from '../groupmember/groupmember.component';
import { MemberService } from '../../services/member.service';
import { Group } from '../../models/group';
import { GroupService } from '../../services/group.service';
import { GroupCategory } from '../../models/groupcategory';

@Component({
    selector: 'groupmy',
    templateUrl: './group.my.component.html',
    styleUrls: ['./group.my.component.css',
        '../table/table.component.css'
    ]
})
export class GroupMyComponent {
    private gmm: GroupMemberModule;

    public group: Group;
    public groupCategories: GroupCategory[];

    public search: Group;
    public pager: Pager;
    public chunk: Chunk;

    public names: string[];
    suggestions: string[] = [];

    suggest(fc: any) {
        if (fc.value.length > 0) {
            this.suggestions = this.names
                .filter(c => c.startsWith(fc.value.toUpperCase()))
                .slice(0, 5);
        }
        else {
            this.suggestions.length = 0;
        }
    }

    suggestionSelect(s: string, f: NgForm, fieldName: string) {
        this.suggestions.length = 0;

        f.controls[fieldName].setValue(s);
    }

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private mm: MemberService, private gm: GroupService) {
        this.gmm = new GroupMemberModule();
        this.gmm.http = http;
        this.gmm.baseUrl = baseUrl;
    }

    async paginate(obj: Group, page: number) {
        this.chunk = await this.gm.search(obj, page) as Chunk;
    }

    public onMembersLoad(groupId: number) {
        // https://stackoverflow.com/questions/19589053/how-to-open-specific-tab-of-bootstrap-nav-tabs-on-click-of-a-particuler-link-usi
        $('.nav-pills a[href="#home"]').tab('show');

        this.onEditInit(groupId);
    }

    async onEditInit(groupId: number) {
        this.group = await this.gm.get(groupId);
    }

    public onPaginate(page: number) {
        this.paginate(this.search, page);
    }

    ngOnInit() {
        this.group = new Group();
        this.search = new Group();
        this.pager = new Pager();
        this.paginate(this.group, 1);

        this.titleService.setTitle('My Agenda - Groups');
    }

    async ngAfterViewInit() {
        var gc = new GroupCategoryModule();
        gc.http = this.http;
        gc.baseUrl = this.baseUrl;
        gc.ex = new ErrorExceptionModule();
        gc.find(new GroupCategory()).subscribe(result => { this.groupCategories = result });

        this.names = await this.mm.allNames();
    }

    ngAfterViewChecked() {
        if (this.chunk) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("#tbl");
            tbl.onscroll = function () {
                $("#tbl > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }
    }

    public onSearchSubmit(f: NgForm) {
        var g = new Group();
        g.name = f.controls['name'].value;
        g.groupCategoryId = f.controls['groupCategoryId'].value;
        g.leader = f.controls['leader'].value;

        if (f.controls['groupCategoryId'].value == "" || f.controls['groupCategoryId'].value == null) {
            g.groupCategoryId = 0;
        }

        this.paginate(g, 1);
        this.search = g;
    }

    async onGroupInfoChange(groupId: number) {   
        if (groupId > 0) {
            var g = await this.gm.get(groupId);

            // Update groups view
            for (let e of this.chunk.groups) {
                if (e.id == groupId) {
                    let index: number = this.chunk.groups.indexOf(e);
                    this.chunk.groups[index] = g;
                }
            }
        }
    }

    async onGroupAdd(groupId: number) {
        if (groupId > 0) {
            var g = await this.gm.get(groupId);

            // Add new group to the list
            //this.chunk.groups.push(g);
            this.chunk.groups.splice(0, 0, g);
            this.chunk.pager.totalItems++;
        }
    }

    join(groupId: number) {
        var gm = new GroupMember();
        gm.groupId = groupId;
        this.gmm.add(gm).subscribe(r => {

            alert("Success");

            this.onGroupInfoChange(groupId);
        });
    }

    leave(groupId: number) {
        var gm = new GroupMember();
        gm.groupId = groupId;
        this.gmm.delete(gm).subscribe(r => {

            alert("Success");

            this.onGroupInfoChange(groupId);
        });
    }

    clearSuggestions() {
        this.suggestions.length = 0;
    }
}

class Chunk {
    groups: Group[];
    pager: Pager;
}