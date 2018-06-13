import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

export class LocationModule {
    public http: Http;
    public baseUrl: string;

    public find(l: Location): Observable<Location[]> {
        return this.http.post(this.baseUrl + 'api/location/find', {
            name: l.name
        }).map(response => response.json());
    }
}

@Component({
    selector: 'location',
    templateUrl: './location.component.html'
})
export class LocationComponent {

}

export class Location {
    id: number;
    name: string;
}