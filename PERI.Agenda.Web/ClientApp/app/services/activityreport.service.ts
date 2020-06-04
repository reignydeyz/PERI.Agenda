import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { ActivityReport } from '../models/activityreport';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ActivityReportService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient) { }

    async generateReport(r: ActivityReport): Promise<string> {
        const response = await this.http.post(this.baseUrl + 'api/activityreport/generatereport', r).toPromise();
        return response as string;
    }
}