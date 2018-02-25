import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';

export class EventCategoryComponent {
    public http: Http;
    public baseUrl: string;

    public find(ec: EventCategory): Observable<EventCategory[]> {
        return this.http.post('api/eventcategory/find', {
            name: ec.name
        }).map(response => response.json());
    }

    //constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
    //    this.http = http;
    //    this.baseUrl = baseUrl;
    //}
}

export class EventCategory {
    id: number;
    name: string;
}