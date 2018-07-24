import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';
import { EventModule, Event } from './event.component';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';
import { EventCategory, EventCategoryModule } from '../eventcategory/eventcategory.component';
import { Location, LocationModule } from '../location/location.component';

@Component({
    selector: 'eventedit',
    templateUrl: './event.edit.component.html'
})
export class EventEditComponent {
    private em: EventModule;
    private ex: ErrorExceptionModule;

    public eventCategories: EventCategory[];
    public locations: Location[];

    @Input('event') event: Event;
    @Output() change: EventEmitter<number> = new EventEmitter<number>();

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        this.em = new EventModule();
        this.em.http = http;
        this.em.baseUrl = baseUrl;

        this.ex = new ErrorExceptionModule();
        this.ex.http = http;
        this.ex.baseUrl = baseUrl;

        if (this.event == null || this.event == undefined) {
            this.event = new Event();
        }
    }

    ngOnInit() {
        var ecm = new EventCategoryModule();
        ecm.http = this.http;
        ecm.baseUrl = this.baseUrl;
        ecm.find(new EventCategory()).subscribe(result => { this.eventCategories = result });

        var l = new LocationModule();
        l.http = this.http;
        l.baseUrl = this.baseUrl;
        l.find(new Location()).subscribe(result => { this.locations = result });
    }

    public onEditSubmit(event: Event) {
        var date = <HTMLInputElement>document.getElementById("txtDate");
        event.dateTimeStart = date.value;

        this.em.edit(event)
            .subscribe(result => {
                this.change.emit(event.id);
                alert('Updated!'); $('#modalEventEdit').modal('toggle');
            }, error => this.ex.catchError(error));
    }
}