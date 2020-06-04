import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Member } from '../models/member';
import { Role } from '../models/role';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AccountService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: HttpClient) { }

    async getProfile(): Promise<Member> {
        const response = await this.http.get(this.baseUrl + 'api/account/profile').toPromise();
        return response as Member;
    }

    async getRole(): Promise<Role> {
        const response = await this.http.get(this.baseUrl + 'api/account/role').toPromise();
        return response as Role;
    }
}