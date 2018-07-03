import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { Observable } from 'rxjs/Observable';

import { GroupCategoryModule, GroupCategory } from '../groupcategory/groupcategory.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Member } from '../member/member.component';
import { Pager } from '../pager/pager.component';
import { Title } from '@angular/platform-browser';

export class GroupModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public find(e: Group): Observable<Group[]> {
        return this.http.post(this.baseUrl + 'api/group/find', {
            name: e.name,
            groupCategoryId: e.groupCategoryId
        }).map(response => response.json());
    }
}

@Component({
    selector: 'group',
    templateUrl: './group.component.html',
    styleUrls: ['./group.component.css',
        '../table/table.component.css'
    ]
})
export class GroupComponent {
    private gm: GroupModule;

    public group: Group;
    public groupCategories: GroupCategory[];

    public search: Group;
    public pager: Pager;
    public chunk: Chunk;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.gm = new GroupModule();
        this.gm.http = http;
        this.gm.baseUrl = baseUrl;

        this.gm.ex = new ErrorExceptionModule();
        this.gm.ex.baseUrl = this.baseUrl;
    }

    private paginate(obj: Group, page: number) {
        this.http.post(this.baseUrl + 'api/group/find/page/' + page, obj).subscribe(result => {
            this.chunk = result.json() as Chunk;
        }, error => this.gm.ex.catchError(error));
    }

    public onMembersLoad(groupId: number) {
        // https://stackoverflow.com/questions/19589053/how-to-open-specific-tab-of-bootstrap-nav-tabs-on-click-of-a-particuler-link-usi
        $('.nav-pills a[href="#home"]').tab('show');

        this.onEditInit(groupId);
    }

    public onEditInit(groupId: number) {
        this.http.get(this.baseUrl + 'api/group/get/' + groupId)
            .subscribe(result => {
                this.group = result.json();
            }, error => this.gm.ex.catchError(error));
    }

    public onPaginate(page: number) {
        this.paginate(this.search, page);
    }

    ngOnInit() {
        this.group = new Group();
        this.search = new Group();
        this.pager = new Pager();
        this.paginate(this.group, 1);

        this.titleService.setTitle('Groups');
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngAfterViewInit() {
        var gc = new GroupCategoryModule();
        gc.http = this.http;
        gc.baseUrl = this.baseUrl;
        gc.ex = this.gm.ex;
        gc.find(new GroupCategory()).subscribe(result => { this.groupCategories = result });
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

        if (f.controls['groupCategoryId'].value == "" || f.controls['groupCategoryId'].value == null) {
            g.groupCategoryId = 0;
        }

        this.paginate(g, 1);
        this.search = g;
    }

    onGroupInfoChange(groupId: number) {
        if (groupId > 0) {
            // Get group
            this.http.get(this.baseUrl + 'api/group/get/' + groupId)
                .subscribe(result => {
                    var g = result.json();

                    // Update groups view
                    for (let e of this.chunk.groups) {
                        if (e.id == groupId) {
                            let index: number = this.chunk.groups.indexOf(e);
                            this.chunk.groups[index] = g;
                        }
                    }

                }, error => this.gm.ex.catchError(error));
        }
    }
}

export class Group {
    id: number;
    groupCategoryId: number;
    category: string;
    name: string;
    members: number;
}

class Chunk {
    groups: Group[];
    pager: Pager;
}