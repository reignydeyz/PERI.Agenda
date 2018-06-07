import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';

export class GroupCategoryModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public find(ec: GroupCategory): Observable<GroupCategory[]> {
        return this.http.post(this.baseUrl + 'api/groupcategory/find', {
            name: ec.name
        }).map(response => response.json());
    }

    public add(gc: GroupCategory): Observable<number> {
        return this.http.post(this.baseUrl + 'api/groupcategory/new', {
            name: gc.name
        }).map(response => response.json());
    }
}

@Component({
    selector: 'groupcategory',
    templateUrl: './groupcategory.component.html',
    styleUrls: ['./groupcategory.component.css',
        '../table/table.component.css']
})
export class GroupCategoryComponent {
    private gcm: GroupCategoryModule;

    public groupcategory = GroupCategory;
    public groupcategories: GroupCategory[];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.gcm = new GroupCategoryModule();
        this.gcm.http = http;
        this.gcm.baseUrl = baseUrl;

        this.gcm.ex = new ErrorExceptionModule();
        this.gcm.ex.baseUrl = this.baseUrl;
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngOnInit() {
        this.titleService.setTitle('Group Categories');

        this.gcm.find(new GroupCategory()).subscribe(result => { this.groupcategories = result });
    }

    ngAfterViewChecked() {
        if (this.groupcategories) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("table");
            tbl.onscroll = function () {
                $("table > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }
    }

    onNewSubmit(f: NgForm) {
        var gc = new GroupCategory();
        gc.name = f.controls['name'].value;

        this.gcm.add(gc).subscribe(
            result => {
                gc.id = result;
                this.groupcategories.push(gc);

                alert('Added!');
                $('#modalNew').modal('toggle');
            },
            error => this.gcm.ex.catchError(error));
    }
}

export class GroupCategory {
    id: number;
    name: string;
    groups: number;
}