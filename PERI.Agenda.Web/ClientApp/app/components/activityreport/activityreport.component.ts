import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";
import * as moment from 'moment';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Title } from '@angular/platform-browser';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { NgForm, NgModel } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { Report, ReportModule } from '../reporttemplate/reporttemplate.component';
import { IMyDpOptions } from 'mydatepicker';

@Component({
    selector: 'activityreport',
    templateUrl: './activityreport.component.html'
})
export class ActivityReportComponent {
    @Input() groupId: number = 0;

    private ex: ErrorExceptionModule;
    private rm: ReportModule;

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };

    reports: Report[] = [];

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
    }

    onSubmit(f: NgForm) {
        var r = new ActivityReport();
        r.groupId = this.groupId;
        r.reportId = f.controls['reportId'].value;
        r.dateTimeStart = f.controls['dateTimeStart'].value.formatted;
        r.dateTimeEnd = f.controls['dateTimeEnd'].value.formatted;

        this.http.post(this.baseUrl + 'api/activityreport/generatereport', r).subscribe(result => {

        });
    }
}

export class ActivityReport {
    reportId: number;
    groupId: number;
    dateTimeStart: string;
    dateTimeEnd: string;
}