import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { EventCategory } from '../models/eventcategory';
import { GraphDataSet } from '../components/graph/graph.component';

@Injectable()
export class EventCategoryService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: Http) { }

    async find(ec: EventCategory): Promise<EventCategory[]> {
        const response = await this.http.post(this.baseUrl + 'api/eventcategory/find', {
            name: ec.name
        }).toPromise();

        return response.json() as EventCategory[];
    }

    async add(ec: EventCategory): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/eventcategory/new', {
            name: ec.name
        }).toPromise();

        return response.json() as number;
    }

    async edit(ec: EventCategory) {
        await this.http.post(this.baseUrl + 'api/eventcategory/edit', {
            id: ec.id,
            name: ec.name
        }).toPromise();
    }

    async get(id: number): Promise<EventCategory> {
        const response = await this.http.get(this.baseUrl + 'api/eventcategory/get/' + id).toPromise();
        return response.json() as EventCategory;
    }

    async delete(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        await this.http.post(this.baseUrl + 'api/eventcategory/delete', body, options).toPromise();
    }

    async download(): Promise<string> {
        const response = await this.http.get(this.baseUrl + 'api/eventcategory/download').toPromise();
        return response.text();
    }

    async stats(id: number): Promise<GraphDataSet> {
        const response = await this.http.get(this.baseUrl + 'api/eventcategory/stats/' + id).toPromise();
        return response.json() as GraphDataSet;
    }

    async events(id: number): Promise<any> {
        const response = await this.http.get(this.baseUrl + 'api/eventcategory/events/' + id).toPromise();
        return response.json();
    }
}