import { Component, Input, Inject, OnChanges, Output, EventEmitter } from '@angular/core';
import * as $ from "jquery";

import { NgForm } from '@angular/forms';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Http } from '@angular/http';
import { EventCategory } from '../../models/eventcategory';
import { EventCategoryService } from '../../services/eventcategory.service';
import { LocationService } from '../../services/location.service';
import { Location } from '../../models/location';
import { Event } from '../../models/event';
import { EventService } from '../../services/event.service';

@Component({
    selector: 'eventedit',
    templateUrl: './event.edit.component.html'
})
export class EventEditComponent {
    public eventCategories: EventCategory[];
    public locations: Location[];

    @Input('event') event: Event;
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

    async onEditSubmit(event: Event) {
        var date = <HTMLInputElement>document.getElementById("txtDate");
        event.dateTimeStart = date.value;

        await this.em.edit(event)
            .then(() => {
                this.change.emit(event.id);
                alert('Updated!'); $('#modalEventEdit').modal('toggle');
            }, error => this.ex.catchError(error));
    }
}