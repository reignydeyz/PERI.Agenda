import { Injectable, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Location } from '../models/location';
import { GraphDataSet } from '../components/graph/graph.component';

@Injectable()
export class LocationService {
    constructor(@Inject('BASE_URL') private baseUrl: string,
        private http: Http) { }

    async add(l: Location): Promise<number> {
        const response = await this.http.post(this.baseUrl + 'api/location/new', {
            name: l.name,
            address: l.address
        }).toPromise();
        return response.json() as number;
    }

    async edit(l: Location) {
        await this.http.post(this.baseUrl + 'api/location/edit', {
            id: l.id,
            name: l.name,
            address: l.address
        }).toPromise();
    }

    async find(l: Location): Promise<Location[]> {
        const response = await this.http.post(this.baseUrl + 'api/location/find', {
            name: l.name
        }).toPromise();
        return response.json() as Location[];
    }

    async get(id: number): Promise<Location> {
        const response = await this.http.get(this.baseUrl + 'api/location/get/' + id).toPromise();
        return response.json() as Location;
    }

    async delete(selectedIds: number[]) {
        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        await this.http.post(this.baseUrl + 'api/location/delete', body, options).toPromise();
    }

    async stats(id: number): Promise<GraphDataSet> {
        const response = await this.http.get(this.baseUrl + 'api/location/stats/' + id).toPromise();
        return response.json() as GraphDataSet;
    }

    async events(id: number): Promise<any> {
        const response = await this.http.get(this.baseUrl + 'api/location/events/' + id).toPromise();
        return response.json();
    }
}