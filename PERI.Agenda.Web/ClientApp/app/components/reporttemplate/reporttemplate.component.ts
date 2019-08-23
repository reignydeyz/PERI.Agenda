import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { Title } from '@angular/platform-browser';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";
import { Observable } from 'rxjs/Observable';
import { Report, ReportEventCategory } from '../../models/report';
import { ReportService } from '../../services/report.service';
import { EventCategory } from '../../models/eventcategory';
import { EventCategoryService } from '../../services/eventcategory.service';

@Component({
    selector: 'reporttemplate',
    templateUrl: './reporttemplate.component.html',
    styleUrls: ['./reporttemplate.component.css',
        '../table/table.component.css']
})
export class ReportTemplateComponent {
    checklist: ReportEventCategory[];
    report: Report;
    reports: Report[] = [];
    eventCategories: EventCategory[] = [];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private rm: ReportService, private ex: ErrorExceptionModule, private ecm: EventCategoryService) {
        
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    async ngOnInit() {
        this.titleService.setTitle('Locations');

        this.reports = await this.rm.find(new Report());
        this.eventCategories = await this.ecm.find(new EventCategory());
    }

    async onEditInit(r: Report) {
        this.report = r;

        this.checklist = await this.rm.checklist(r.reportId);
    }

    async onEditSubmit(rpt: Report) {
        await this.rm.edit(rpt).then(() => {
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
        });
    }

    async onNewSubmit(f: NgForm) {
        var r = new Report();
        r.name = f.controls['name'].value;

        await this.rm.add(r).then(response => {
            r.reportId = response;

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
        });
    }

    async onDeleteClick() {
        var flag = confirm('Are you sure you want to delete selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('#tbl input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        await this.rm.delete(selectedIds).then(() => {
            for (let id of selectedIds) {
                for (let e of this.reports) {
                    if (e.reportId == id) {
                        this.reports.splice(this.reports.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');
        }).catch(error => {
            this.ex.catchError(error);
        });
    }
}