import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { Http, Headers, RequestOptions } from '@angular/http';
import { Title } from '@angular/platform-browser';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { NgForm } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { Report, ReportModule } from '../reporttemplate/reporttemplate.component';

@Component({
    selector: 'activityreport',
    templateUrl: './activityreport.component.html'
})
export class ActivityReportComponent {
    @Input() groupId: number = 0;

    private ex: ErrorExceptionModule;
    private rm: ReportModule;

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
}