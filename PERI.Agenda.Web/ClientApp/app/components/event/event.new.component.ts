import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';
import { EventModule, Event } from './event.component';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';
import { EventCategory, EventCategoryModule } from '../eventcategory/eventcategory.component';
import { Location, LocationModule } from '../location/location.component';
import { Group } from '../../models/group';

@Component({
    selector: 'eventnew',
    templateUrl: './event.new.component.html'
})
export class EventNewComponent {
    private em: EventModule;
    private ex: ErrorExceptionModule;

    public eventCategories: EventCategory[];
    public locations: Location[];

    @Input('event') event: Event;
    @Input('group') group: Group;
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

    public onNewSubmit(f: NgForm) {
        var e = new Event();
        e.name = f.controls['name'].value;
        e.eventCategoryId = f.controls['eventCategoryId'].value;
        e.dateTimeStart = f.controls['dateTimeStart'].value;
        e.locationId = f.controls['locationId'].value;
        e.attendance = 0;

        let ec: any = this.eventCategories.find(x => x.id == e.eventCategoryId);
        e.category = ec.name;

        let l: any = this.locations.find(x => x.id == e.locationId);
        e.location = l.name;

        if (this.group == null || this.group == undefined) {
            this.em.add(e).subscribe(
                result => {
                    e.id = result;
                    this.change.emit(e.id);

                    alert('Added!');
                    $('#modalEventNew').modal('toggle');
                }, error => this.ex.catchError(error));
        }
        else {
            this.em.addExclusive(e, this.group.id).subscribe(
                result => {
                    e.id = result;
                    this.change.emit(e.id);

                    alert('Added!');
                    $('#modalEventNew').modal('toggle');
                }, error => this.ex.catchError(error));
        }
    }
}