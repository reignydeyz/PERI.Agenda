import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';

export class EventCategoryModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;
    public token: string;

    public find(ec: EventCategory): Observable<EventCategory[]> {
        let headers = new Headers();
        headers.append('Authorization', this.token);

        return this.http.post(this.baseUrl + 'api/eventcategory/find', {
            name: ec.name
        }, { headers: headers }).map(response => response.json());
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

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ecm = new EventCategoryModule();
        this.ecm.http = http;
        this.ecm.baseUrl = baseUrl;

        this.ecm.ex = new ErrorExceptionModule();
        this.ecm.ex.baseUrl = this.baseUrl;

        this.ecm.token = (<HTMLInputElement>document.getElementById("hToken")).value;
    }

    ngOnInit() {
        this.ecm.find(new EventCategory()).subscribe(result => { this.eventcategories = result }, error => this.ecm.ex.catchError(error));
        this.titleService.setTitle('Event Categories');
    }
}

export class EventCategory {
    id: number;
    name: string;
    events: number;
    minAttendees: number;
    averageAttendees: number;
    maxAttendees: number;
}