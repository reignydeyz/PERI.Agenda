import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { EventCategoryComponent, EventCategory } from '../eventcategory/eventcategory.component';

@Component({
    selector: 'event',
    templateUrl: './event.component.html'
})
export class EventComponent {
    public event: Event;
    public events: Event[];
    public eventCategories: EventCategory[];

    _http: Http;
    _baseUrl: string;

    private find(e: Event) {
        this._http.post(this._baseUrl + 'api/event/find', {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart
        }).subscribe(result => {
            this.events = result.json() as Event[];
        }, error => console.error(error));
    }

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this._http = http;
        this._baseUrl = baseUrl;
        this.event = new Event();
        this.find(new Event());
    }

    ngAfterViewInit() {
        var ecc = new EventCategoryComponent();
        ecc.http = this._http;
        ecc.baseUrl = this._baseUrl;
        ecc.find(new EventCategory()).subscribe(result => { this.eventCategories = result });        
    }

    public onSearchSubmit(f: NgForm) {
        var m = new Event();
        m.name = f.controls['name'].value;
        m.eventCategoryId = f.controls['eventCategoryId'].value;
        m.dateTimeStart = f.controls['dateTimeStart'].value;

        this.find(m);
    }
}

export class Event {
    id: number;
    eventCategoryId: number;
    category: string;
    name: string;
    dateTimeStart: string;
    time: string;
    location: string;
    attendance: number;
}
