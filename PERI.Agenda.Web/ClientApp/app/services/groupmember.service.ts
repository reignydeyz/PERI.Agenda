import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { GroupMember } from '../models/groupmember';
import { Member } from '../models/member';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class GroupMemberService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient) { }

    async add(gm: GroupMember): Promise<number> {
        const response = await this.http.put(this.baseUrl + 'api/groupmember/add', {
            memberId: gm.memberId,
            groupId: gm.groupId
        }).toPromise();
        return response as number;
    }

    async delete(gm: GroupMember) {
        await this.http.post(this.baseUrl + 'api/groupmember/delete', {
            memberId: gm.memberId,
            groupId: gm.groupId
        }).toPromise();
    }

    async find(id: number, member: Member): Promise<Member[]> {
        const response = await this.http.post(this.baseUrl + 'api/groupmember/find/' + id, member).toPromise();
        return response as Member[];
    }

    async search(id: number, member: string, page: number): Promise<any> {
        let body = JSON.stringify(member);
        /*let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });*/

        const response = await this.http.post(this.baseUrl + 'api/groupmember/' + id + '/checklist/page/' + page, body, { "headers": { "Content-Type": "application/json" } }).toPromise();
        return response;
    }

    async download(groupId: number): Promise<string> {
        const response = await this.http.get(this.baseUrl + 'api/groupmember/' + groupId + '/download').toPromise();
        return response as string;
    }
}