import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Member } from '../models/member';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class MemberService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient, private http1: Http) { }

    async add(m: Member) : Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/member/new', {
            name: m.name,
            nickName: m.nickName,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
            isActive: m.isActive,
            gender: m.gender,
            invitedByMemberName: m.invitedByMemberName,
            remarks: m.remarks
        }).toPromise();

        return response as number;
    }

    async allNames(): Promise<string[]> {
        const response = await this.http.get(this.baseUrl + 'api/member/allnames').toPromise();
        return response as string[];
    }

    async leading(id: number): Promise<number> {
        const response = await this.http.get(this.baseUrl + 'api/member/' + id + '/leading').toPromise();
        return response as number;
    }

    async following(id: number): Promise<number> {
        const response = await this.http.get(this.baseUrl + 'api/member/' + id + '/following').toPromise();
        return response as number;
    }

    async invites(id: number): Promise<number> {
        const response = await this.http.get(this.baseUrl + 'api/member/' + id + '/invites').toPromise();
        return response as number;
    }

    async activities(id: number): Promise<any[]> {
        const response = await this.http.get(this.baseUrl + 'api/member/' + id + '/activities').toPromise();
        return response as any[];
    }

    async updateRole(m: Member) {
        await this.http.post(this.baseUrl + 'api/user/updaterole', m).toPromise();
    }

    async edit(m: Member) {
        await this.http.post(this.baseUrl + 'api/member/edit', {
            id: m.id,
            name: m.name,
            nickName: m.nickName,
            gender: m.gender,
            birthDate: m.birthDate,
            email: m.email,
            address: m.address,
            mobile: m.mobile,
            isActive: m.isActive,
            invitedByMembername: m.invitedByMemberName,
            remarks: m.remarks
        }).toPromise();
    }

    async get(id: number): Promise<Member> {
        const response = await this.http.get(this.baseUrl + 'api/member/get/' + id).toPromise();
        return response as Member;
    }

    async delete(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        /*let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        await this.http.post(this.baseUrl + 'api/member/delete', body, { "headers": { "Content-Type": "application/json" } }).toPromise();
    }

    async activate(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        /*et headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        await this.http.post(this.baseUrl + 'api/member/activate', body, { "headers": { "Content-Type": "application/json" } }).toPromise();
    }

    async deactivate(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        /*let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        await this.http.post(this.baseUrl + 'api/member/deactivate', body, { "headers": { "Content-Type": "application/json" } }).toPromise();
    }

    async getTotal(args: string) : Promise<number> {
        const response = await this.http.get(this.baseUrl + 'api/member/total/' + args).toPromise();
        return response as number;
    }

    async find(m: Member, page: number) : Promise<any> {
        const response = await this.http.post(this.baseUrl + 'api/member/find/page/' + page, m).toPromise();
        return response as any;
    }

    async download(m: Member): Promise<string> {
        const response = await this.http1.post(this.baseUrl + 'api/member/download/', m).toPromise();
        return response.text();
    }
}