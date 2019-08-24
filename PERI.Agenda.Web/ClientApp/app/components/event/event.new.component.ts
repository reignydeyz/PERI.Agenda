import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';
import { Group } from '../../models/group';
import { EventCategory } from '../../models/eventcategory';
import { EventCategoryService } from '../../services/eventcategory.service';
import { LocationService } from '../../services/location.service';
import { Location } from '../../models/location';
import { Event } from '../../models/event';
import { EventService } from '../../services/event.service';

@Component({
    selector: 'eventnew',
    templateUrl: './event.new.component.html'
})
export class EventNewComponent {
    public eventCategories: EventCategory[];
    public locations: Location[];

    @Input('event') event: Event;
    @Input('group') group: Group;
    @Output() change: EventEmitter<number> = new EventEmitter<number>();

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string,
    private ecm: EventCategoryService, private lm: LocationService, private em: EventService, private ex: ErrorExceptionModule) {
       
        if (this.event == null || this.event == undefined) {
            this.event = new Event();
        }
    }

    async ngOnInit() {
        this.eventCategories = await this.ecm.find(new EventCategory());
        this.locations = await this.lm.find(new Location());
    }

    async onNewSubmit(f: NgForm) {
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
            await this.em.add(e).then(
                result => {
                    e.id = result;
                    this.change.emit(e.id);

                    alert('Added!');
                    $('#modalEventNew').modal('toggle');
                }, error => this.ex.catchError(error));
        }
        else {
            await this.em.addExclusive(e, this.group.id).then(
                result => {
                    e.id = result;
                    this.change.emit(e.id);

                    alert('Added!');
                    $('#modalEventNew').modal('toggle');
                }, error => this.ex.catchError(error));
        }
    }
}