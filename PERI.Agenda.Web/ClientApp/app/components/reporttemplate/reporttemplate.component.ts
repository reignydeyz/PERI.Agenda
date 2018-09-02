import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { Title } from '@angular/platform-browser';
import { NgForm, NgModel } from '@angular/forms';
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
        return this.http.post(this.baseUrl + 'api/reporttemplate/edit', r);
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

    checklist: ReportEventCategory[];
    report: Report;
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

    onEditInit(r: Report) {
        this.report = r;

        this.checklist = [];

        this.http.get(this.baseUrl + 'api/reporttemplate/checklist/' + r.reportId)
            .subscribe(result => {
                this.checklist = result.json();
            }, error => this.rm.ex.catchError(error));
    }

    onEditSubmit(rpt: Report) {
        this.rm.edit(rpt).subscribe(r => {

            var selectedIds = new Array();
            $("#frmEdit input:checkbox:checked").each(function () {
                if ($(this).prop('checked')) {
                    selectedIds.push($(this).val());
                }
            });

            let body = JSON.stringify(selectedIds);
            let headers = new Headers({ 'Content-Type': 'application/json' });
            let options = new RequestOptions({ headers: headers });
            
            this.http.post(this.baseUrl + 'api/eventcategoryreport/update/' + rpt.reportId, body, options)
                .subscribe(res => {
                    alert('Updated.');
                    $('#modalEdit').modal('toggle');
                });
        })
    }

    onNewSubmit(f: NgForm) {
        var r = new Report();
        r.name = f.controls['name'].value;

        this.rm.add(r).subscribe(
            result => {
                r.reportId = result;

                var selectedIds = new Array();
                $("#frmNew input:checkbox:checked").each(function () {
                    if ($(this).prop('checked')) {
                        selectedIds.push($(this).val());
                    }
                });

                let body = JSON.stringify(selectedIds);
                let headers = new Headers({ 'Content-Type': 'application/json' });
                let options = new RequestOptions({ headers: headers });
                
                this.http.post(this.baseUrl + 'api/eventcategoryreport/addrange/' + r.reportId, body, options)
                    .subscribe(res => {
                        this.reports.push(r);

                        alert('Added!');
                        $('#modalNew').modal('toggle');
                    });
            },
            error => this.rm.ex.catchError(error));
    }

    onDeleteClick() {
        var flag = confirm('Are you sure you want to delete selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('#tbl input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/reporttemplate/delete', body, options).subscribe(result => {

            for (let id of selectedIds) {
                for (let e of this.reports) {
                    if (e.reportId == id) {
                        this.reports.splice(this.reports.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');

        }, error => this.rm.ex.catchError(error));
    }
}

export class Report {
    reportId: number;
    name: string;
}

export class ReportEventCategory {
    id: number;
    name: number;
    isSelected: boolean;
}