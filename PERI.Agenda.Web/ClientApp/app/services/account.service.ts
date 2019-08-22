import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Member } from '../models/member';
import { Role } from '../models/role';

@Injectable()
export class AccountService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: Http) { }

    async getProfile(): Promise<Member> {
        const response = await this.http.get(this.baseUrl + 'api/account/profile').toPromise();
        return response.json() as Member;
    }

    async getRole(): Promise<Role> {
        const response = await this.http.get(this.baseUrl + 'api/account/role').toPromise();
        return response.json() as Role;
    }
}