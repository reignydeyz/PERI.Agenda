import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel, FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import * as $ from "jquery";

import { Observable } from 'rxjs/Observable';

import { GroupCategoryModule, GroupCategory } from '../groupcategory/groupcategory.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Member } from '../member/member.component';
import { Pager } from '../pager/pager.component';
import { Title } from '@angular/platform-browser';
import { MemberModule } from '../member/member.component';

export class GroupModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public find(e: Group): Observable<Group[]> {
        return this.http.post(this.baseUrl + 'api/group/find', {
            name: e.name,
            groupCategoryId: e.groupCategoryId,
            leader: e.leader
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
    private mm: MemberModule;

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

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.gm = new GroupModule();
        this.gm.http = http;
        this.gm.baseUrl = baseUrl;

        this.gm.ex = new ErrorExceptionModule();
        this.gm.ex.baseUrl = this.baseUrl;

        this.mm = new MemberModule();
        this.mm.http = http;
        this.mm.baseUrl = baseUrl;
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

        this.mm.allNames().subscribe(result => { this.names = result });
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

    clearSuggestions() {
        this.suggestions.length = 0;
    }
}

export class Group {
    id: number;
    groupCategoryId: number;
    category: string;
    name: string;
    members: number;
    leader: string;
}

class Chunk {
    groups: Group[];
    pager: Pager;
}