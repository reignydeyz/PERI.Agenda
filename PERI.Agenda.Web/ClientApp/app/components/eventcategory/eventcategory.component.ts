﻿import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';

export class EventCategoryModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public find(ec: EventCategory): Observable<EventCategory[]> {
        return this.http.post(this.baseUrl + 'api/eventcategory/find', {
            name: ec.name
        }).map(response => response.json());
    }

    public get(id: number): Observable<EventCategory> {
        return this.http.get(this.baseUrl + 'api/eventcategory/get/' + id).map(response => response.json());
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
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngOnInit() {
        this.titleService.setTitle('Event Categories');
        this.ecm.find(new EventCategory()).subscribe(result => { this.eventcategories = result }, error => this.ecm.ex.catchError(error));        
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