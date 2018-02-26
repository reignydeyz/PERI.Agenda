import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';

export class EventCategoryModule {
    public http: Http;
    public baseUrl: string;

    public find(ec: EventCategory): Observable<EventCategory[]> {
        return this.http.post('api/eventcategory/find', {
            name: ec.name
        }).map(response => response.json());
    }
}

@Component({
    selector: 'eventcategory',
    templateUrl: './eventcategory.component.html'
})
export class EventCategoryComponent {
    private ecm: EventCategoryModule;

    public eventcategory = EventCategory;
    public eventcategories: EventCategory[];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        this.ecm = new EventCategoryModule();
        this.ecm.http = http;
        this.ecm.baseUrl = baseUrl;

        this.ecm.find(new EventCategory()).subscribe(result => { this.eventcategories = result });
    }
}

export class EventCategory {
    id: number;
    name: string;
    events: number;
}