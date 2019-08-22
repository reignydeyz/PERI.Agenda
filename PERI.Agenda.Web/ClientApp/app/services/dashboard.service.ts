import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Statistics, GraphDataSet } from '../components/graph/graph.component';


@Injectable()
export class DashboardService {

    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: Http) { }

    async eventcategoryStats(): Promise<Statistics> {
        const response = await this.http.get(this.baseUrl + 'api/dashboard/eventcategories').toPromise();
        return response.json() as Statistics;
    }

    async locationStats(): Promise<Statistics> {
        const response = await this.http.get(this.baseUrl + 'api/dashboard/locations').toPromise();
        return response.json() as Statistics;
    }

    async memberStats(): Promise<Statistics> {
        const response = await this.http.get(this.baseUrl + 'api/dashboard/member').toPromise();
        return response.json() as Statistics;
    }

    async groupCategoryStats(): Promise<GraphDataSet> {
        const response = await this.http.get(this.baseUrl + 'api/dashboard/groupcategories').toPromise();
        return response.json() as GraphDataSet;
    }
}
