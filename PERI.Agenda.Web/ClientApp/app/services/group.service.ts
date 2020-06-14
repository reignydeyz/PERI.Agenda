import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Group } from '../models/group';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class GroupService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient, private http1: Http) { }

    async find(e: Group): Promise<Group[]> {
        const response = await this.http.post(this.baseUrl + 'api/group/find', {
            name: e.name,
            groupCategoryId: e.groupCategoryId,
            leader: e.leader
        }).toPromise();

        return response as Group[];
    }

    async search(e: Group, page: number): Promise<any> {
        const response = await this.http.post(this.baseUrl + 'api/group/find/page/' + page, e).toPromise();
        return response;
    }

    async add(g: Group): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/group/new', {
            name: g.name,
            groupCategoryId: g.groupCategoryId,
            leader: g.leader
        }).toPromise();

        return response as number;
    }

    async edit(g: Group) {
        await this.http.post(this.baseUrl + 'api/group/edit', {
            id: g.id,
            name: g.name,
            groupCategoryId: g.groupCategoryId,
            leader: g.leader
        }).toPromise();
    }

    async get(id: number) : Promise<Group> {
        const response = await this.http.get(this.baseUrl + 'api/group/get/' + id).toPromise();
        return response as Group;
    }

    async delete(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        /*let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        await this.http.post(this.baseUrl + 'api/group/delete', body, { "headers": { "Content-Type": "application/json" } }).toPromise();
    }

    async download(g: Group): Promise<string> {
        const response = await this.http1.post(this.baseUrl + 'api/group/download', g).toPromise();
        return response.text();
    }

    async downloadMembers(groupId: number): Promise<string> {
        const response = await this.http1.get(this.baseUrl + 'api/groupmember/' + groupId + '/download').toPromise();
        return response.text();
    }
}