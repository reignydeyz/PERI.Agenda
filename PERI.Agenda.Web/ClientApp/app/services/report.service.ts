import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Report, ReportEventCategory } from '../models/report';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ReportService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient) { }

    async add(r: Report): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/reporttemplate/new', r).toPromise();
        return response as number;
    }

    async edit(r: Report) {
        await this.http.post(this.baseUrl + 'api/reporttemplate/edit', r).toPromise();
    }

    async find(r: Report): Promise<Report[]> {
        const response = await this.http.post(this.baseUrl + 'api/reporttemplate/find', r).toPromise();
        return response as Report[];
    }

    async checklist(id: number): Promise<ReportEventCategory[]> {
        const response = await this.http.get(this.baseUrl + 'api/reporttemplate/checklist/' + id).toPromise();
        return response as ReportEventCategory[];
    }

    async delete(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        /*let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        await this.http.post(this.baseUrl + 'api/reporttemplate/delete', body, { "headers": { "Content-Type": "application/json" } }).toPromise();
    }
}