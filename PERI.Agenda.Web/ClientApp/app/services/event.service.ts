import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Event } from '../models/event';

@Injectable()
export class EventService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: Http) { }

    async find(e: Event): Promise<Event[]> {
        const response = await this.http.post(this.baseUrl + 'api/event/find', {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            dateTimeEnd: e.dateTimeEnd,
            locationId: e.locationId
        }).toPromise();
        return response.json() as Event[];
    }

    async add(e: Event): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/event/new', {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).toPromise();

        return response.json() as number;
    }

    async addExclusive(e: Event, groupId: number): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/event/new/exclusive/' + groupId, {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).toPromise();

        return response.json() as number;
    }

    async edit(e: Event) {
        await this.http.post(this.baseUrl + 'api/event/edit', {
            id: e.id,
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).toPromise();
    }

    async get(id: number): Promise<Event> {
        const response = await this.http.get(this.baseUrl + 'api/event/get/' + id).toPromise();
        return response.json() as Event;
    }

    async search(obj: Event, page: number): Promise<any> {
        const response = await this.http.post(this.baseUrl + 'api/event/find/page/' + page, obj).toPromise();
        return response.json();
    }

    async searchMyPage(obj: Event, page: number): Promise<any> {
        const response = await this.http.post(this.baseUrl + 'api/event/find/mypage/' + page, obj).toPromise();
        return response.json();
    }

    async download(e: Event): Promise<string> {
        const response = await this.http.post(this.baseUrl + 'api/event/download', e).toPromise();
        return response.text();
    }

    async delete(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        await this.http.post(this.baseUrl + 'api/event/delete', body, options).toPromise();
    }
}