import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { Title } from '@angular/platform-browser';
import * as $ from "jquery";
import { Observable } from 'rxjs/Observable';
import { EventCategory, EventCategoryModule } from '../eventcategory/eventcategory.component';

export class ReportModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public add(r: Report): Observable<number> {
        return this.http.post(this.baseUrl + 'api/reporttemplate/new', r).map(response => response.json());
    }

    public edit(r: Report) {
        return this.http.post(this.baseUrl + 'api/reporttemplate/edit', r).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => this.ex.catchError(error));
    }

    public find(r: Report): Observable<Report[]> {
        return this.http.post(this.baseUrl + 'api/reporttemplate/find', r).map(response => response.json());
    }
}

@Component({
    selector: 'reporttemplate',
    templateUrl: './reporttemplate.component.html',
    styleUrls: ['./reporttemplate.component.css',
        '../table/table.component.css']
})
export class ReportTemplateComponent {
    rm: ReportModule;
    ecm: EventCategoryModule;
    reports: Report[] = [];
    eventCategories: EventCategory[] = [];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.rm = new ReportModule();
        this.rm.http = http;
        this.rm.baseUrl = baseUrl;
        this.rm.ex = new ErrorExceptionModule();

        this.ecm = new EventCategoryModule();
        this.ecm.http = http;
        this.ecm.baseUrl = baseUrl;
        this.ecm.ex = new ErrorExceptionModule();
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngOnInit() {
        this.titleService.setTitle('Locations');

        this.rm.find(new Report()).subscribe(r => {
            this.reports = r
        }, err => this.rm.ex.catchError(err));

        this.ecm.find(new EventCategory()).subscribe(r => {
            this.eventCategories = r
        }, err => this.rm.ex.catchError(err));
    }
}

export class Report {
    reportId: string;
    name: string;
}