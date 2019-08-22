import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class RoleService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: Http) { }

    async getAll() : Promise<any> {
        const response = await this.http.get(this.baseUrl + 'api/role/getall').toPromise();
        return response.json();
    }
}