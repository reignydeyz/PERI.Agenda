import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

import { Event } from '../event/event.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { Title } from '@angular/platform-browser';
import * as moment from "moment";

@Component({
    selector: 'calendar',
    templateUrl: './calendar.component.html'
})
export class CalendarComponent {
    public events: Event[];

    private ex: ErrorExceptionModule;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ex = new ErrorExceptionModule();
    }

    ngAfterViewInit() {
        this.http.get(this.baseUrl + 'api/calendar/events').subscribe(result => {
            this.events = result.json() as Event[];
        }, error => this.ex.catchError(error));
    }
}