import { Component, Input, Inject } from '@angular/core';
import * as $ from "jquery";

import { Http, Headers, RequestOptions } from '@angular/http';
import { Title } from '@angular/platform-browser';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { NgForm, NgModel } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { Report, ReportModule } from '../reporttemplate/reporttemplate.component';

import { saveAs } from 'file-saver';
import { IMyDpOptions } from 'mydatepicker';

@Component({
    selector: 'activityreport',
    templateUrl: './activityreport.component.html'
})
export class ActivityReportComponent {
    @Input() report: ActivityReport;

    private ex: ErrorExceptionModule;
    private rm: ReportModule;
    
    reports: Report[] = [];

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ex = new ErrorExceptionModule();
        this.ex.http = http;
        this.ex.baseUrl = baseUrl;

        this.rm = new ReportModule();
        this.rm.baseUrl = baseUrl;
        this.rm.http = http;
    } 

    ngOnInit() {
        this.rm.find(new Report()).subscribe(r => {
            this.reports = r;
        });


        console.log(this.report);
    }

    onSubmit(r: ActivityReport) {
        this.http.post(this.baseUrl + 'api/activityreport/generatereport', r).subscribe(result => {
            let parsedResponse = result.text();
            this.downloadFile(parsedResponse);
        }, err => this.ex.catchError(err));
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }
}

export class ActivityReport {
    reportId: number;
    groupId: number;
    dateTimeStart: any;
    dateTimeEnd: any;
}