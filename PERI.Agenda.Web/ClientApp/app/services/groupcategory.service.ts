﻿import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { GroupCategory } from '../models/groupcategory';
import { GraphDataSet } from '../components/graph/graph.component';

@Injectable()
export class GroupCategoryService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: Http) { }

    async find(ec: GroupCategory): Promise<GroupCategory[]> {
        const response = await this.http.post(this.baseUrl + 'api/groupcategory/find', {
            name: ec.name
        }).toPromise();

        return response.json() as GroupCategory[];
    }

    async add(gc: GroupCategory): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/groupcategory/new', {
            name: gc.name
        }).toPromise();

        return response.json() as number;
    }

    async edit(gc: GroupCategory): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/groupcategory/edit', {
            id: gc.id,
            name: gc.name
        }).toPromise();

        return response.json() as number;
    }

    async download(id: number): Promise<string> {
        const response = await this.http.get(this.baseUrl + 'api/groupcategory/' + id + '/download').toPromise();
        return response.text();
    }

    async get(gcId: number): Promise<GroupCategory> {
        const response = await this.http.get(this.baseUrl + 'api/groupcategory/get/' + gcId).toPromise();
        return response.json() as GroupCategory;
    }

    async delete(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        await this.http.post(this.baseUrl + 'api/groupcategory/delete', body, options).toPromise();
    }

    async stats(id: number): Promise<GraphDataSet> {
        const response = await this.http.get(this.baseUrl + 'api/groupcategory/stats/' + id).toPromise();
        return response.json() as GraphDataSet;
    }
}