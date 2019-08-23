import { Component, Input, Inject } from '@angular/core';
import * as $ from "jquery";

import { Http, Headers, RequestOptions } from '@angular/http';
import { Title } from '@angular/platform-browser';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { NgForm, NgModel } from '@angular/forms';
import { Observable } from 'rxjs/Observable';

import { saveAs } from 'file-saver';
import { IMyDpOptions } from 'mydatepicker';
import { ActivityReport } from '../../models/activityreport';
import { ActivityReportService } from '../../services/activityreport.service';
import { Report } from '../../models/report';
import { ReportService } from '../../services/report.service';

@Component({
    selector: 'activityreport',
    templateUrl: './activityreport.component.html'
})
export class ActivityReportComponent {
    @Input() report: ActivityReport;
    
    reports: Report[] = [];

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private activityReportService: ActivityReportService, private ex: ErrorExceptionModule, private rm: ReportService) {
    } 

    async ngOnInit() {
        this.reports = await this.rm.find(new Report());
    }

    async onSubmit(r: ActivityReport) {
        this.downloadFile(await this.activityReportService.generateReport(r));
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }
}