import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ActivatedRoute } from '@angular/router';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { Title } from '@angular/platform-browser';
import * as moment from "moment";
import { RsvpModule } from '../rsvp/rsvp.component';
import { Rsvp } from '../../models/rsvp';

@Component({
    selector: 'calendar',
    templateUrl: './calendar.component.html'
})
export class CalendarComponent {
    private rm: RsvpModule;

    public calendarEvents: CalendarEvent[];

    private ex: ErrorExceptionModule;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.ex = new ErrorExceptionModule();

        this.rm = new RsvpModule();
        this.rm.http = http;
        this.rm.baseUrl = baseUrl;
    }

    ngOnInit() {
        this.titleService.setTitle('Calendar');
    }

    ngAfterViewInit() {
        this.http.get(this.baseUrl + 'api/calendar/events').subscribe(result => {
            this.calendarEvents = result.json() as CalendarEvent[];
        }, error => this.ex.catchError(error));
    }

    makeRsvp(r: Rsvp) {
        this.rm.add(r).subscribe(result => {

            for (let rec of this.calendarEvents) {
                if (rec.id == r.eventId) {
                    let index: number = this.calendarEvents.indexOf(rec);
                    this.calendarEvents[index].isGoing = r.isGoing;
                }
            }

            alert('Success');
        }, error => this.ex.catchError(error));
    }

    onGoingClick(eventId: number) {
        let r: Rsvp = new Rsvp();
        r.eventId = eventId;
        r.isGoing = true;
        this.makeRsvp(r);
    }

    onNotInterestedClick(eventId: number) {
        let r: Rsvp = new Rsvp();
        r.eventId = eventId;
        r.isGoing = false;
        this.makeRsvp(r);
    }
}

export class CalendarEvent {
    id: number;
    eventCategoryId: number;
    category: string;
    event: string;
    dateTimeStart: string;
    memberId: number;
    isGoing: boolean;
}