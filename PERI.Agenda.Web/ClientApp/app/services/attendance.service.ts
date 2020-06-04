import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Attendance } from '../models/attendance';
import { Rsvp } from '../models/rsvp';
import * as moment from 'moment';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AttendanceService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient) { }

    async registrants(eventId: number): Promise<Attendance[]> {
        const response = await this.http.get(this.baseUrl + 'api/attendance/' + eventId).toPromise();
        return response as Attendance[];
    }

    async searchRegistrants(eventId: number, member: string): Promise<Attendance[]> {
        let body = JSON.stringify(member);
        /*let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        const response = await this.http.post(this.baseUrl + 'api/attendance/' + eventId + '/search', body, { "headers": { "Content-Type": "application/json" } }).toPromise();
        return response as Attendance[];
    }

    async searchGoing(r: Rsvp): Promise<Rsvp[]> {
        const response = await this.http.post(this.baseUrl + 'api/rsvp/find', {
            eventid: r.eventId,
            member: r.member,
            isGoing: r.isGoing
        }).toPromise();
        return response as Rsvp[];
    }

    async add(eventId: number, a: Attendance): Promise<number> {
        const response = await this.http.put(this.baseUrl + 'api/attendance/' + eventId + '/add', {
            memberId: a.memberId,
            dateTimeLogged: moment().format('MM/DD/YYYY, h:mm:ss a')
        }).toPromise();
        return response as number;
    }

    async delete(eventId: number, a: Attendance) {
        await this.http.post(this.baseUrl + 'api/attendance/' + eventId + '/delete', {
            memberId: a.memberId
        }).toPromise();
    }

    async downloadAttendees(eventId: number): Promise<string> {
        const response = await this.http.get(this.baseUrl + 'api/attendance/' + eventId + '/downloadattendees').toPromise();
        return response as string;
    }

    async downloadFirstTimers(eventId: number): Promise<string> {
        const response = await this.http.get(this.baseUrl + 'api/attendance/' + eventId + '/downloadfirsttimers').toPromise();
        return response as string;
    }

    async getTotal(eventId: number, args: string): Promise<number> {
        const response = await this.http.get(this.baseUrl + 'api/attendance/' + eventId + '/total/' + args).toPromise();
        return response as number;
    }

    async search(eventId: number, obj: string, page: number): Promise<any> {
        let body = JSON.stringify(obj);
        /*let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        const response = await this.http.post(this.baseUrl + 'api/attendance/' + eventId + '/search/page/' + page, body, { "headers": { "Content-Type": "application/json" } }).toPromise();
        return response;
    }

    async paginateAttendees(eventId: number, page: number): Promise<any> {
        const response = await this.http.get(this.baseUrl + 'api/attendance/' + eventId + '/attendees/page/' + page).toPromise();
        return response;
    }

    async firstTimers(eventId: number): Promise<Attendance[]> {
        const response = await this.http.get(this.baseUrl + 'api/attendance/' + eventId + '/firsttimers').toPromise();
        return response as Attendance[];
    }
}