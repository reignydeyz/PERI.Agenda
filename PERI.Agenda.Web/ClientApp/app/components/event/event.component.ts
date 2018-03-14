import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { EventCategoryModule, EventCategory } from '../eventcategory/eventcategory.component';
import { Title } from '@angular/platform-browser';
import * as moment from "moment";
import { LocationModule, Location } from '../location/location.component';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'event',
    templateUrl: './event.component.html'
})
export class EventComponent {
    public event: Event;
    public events: Event[];
    public eventCategories: EventCategory[];
    public locations: Location[];

    private find(e: Event) {
        this.http.post(this.baseUrl + 'api/event/find', {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).subscribe(result => {
            this.events = result.json() as Event[];
        }, error => console.error(error));
    }

    private add(e: Event): Observable<number> {
        return this.http.post(this.baseUrl + 'api/event/new', {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).map(response => response.json());
    }

    // https://stackoverflow.com/questions/44000162/how-to-change-title-of-a-page-using-angularangular-2-or-4-route
    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        
    }

    ngOnInit() {
        this.event = new Event();
        this.find(new Event());

        this.titleService.setTitle('Events');
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngAfterViewInit() {
        var ecc = new EventCategoryModule();
        ecc.http = this.http;
        ecc.baseUrl = this.baseUrl;
        ecc.find(new EventCategory()).subscribe(result => { this.eventCategories = result });        

        var l = new LocationModule();
        l.http = this.http;
        l.baseUrl = this.baseUrl;
        l.find(new Location()).subscribe(result => { this.locations = result });        
    }

    public onSearchSubmit(f: NgForm) {
        var e = new Event();
        e.name = f.controls['name'].value;
        e.eventCategoryId = f.controls['eventCategoryId'].value;
                
        if (f.controls['eventCategoryId'].value == "" || f.controls['eventCategoryId'].value == null) {
            e.eventCategoryId = 0;
        }        
        
        e.dateTimeStart = f.controls['dateTimeStart'].value;
        e.locationId = f.controls['locationId'].value;

        this.find(e);
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

        this.add(e).subscribe(
            result => {
                e.id = result;
                this.events.push(e);

                alert('Added!');
                $('#modalNew').modal('toggle');
            },
            error => {
                console.error(error);
                alert('Oops! Unknown error has occured.')
            });
    }

    public onEditInit(id: number) {
        this.http.get(this.baseUrl + 'api/event/get/' + id)
            .subscribe(result => { this.event = result.json() as Event }, error => console.error(error));
    }
}

export class Event {
    id: number;
    eventCategoryId: number;
    category: string;
    name: string;
    dateTimeStart: string;
    time: string;
    locationId: number;
    location: string;
    attendance: number;
}
