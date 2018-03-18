import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { EventCategoryModule, EventCategory } from '../eventcategory/eventcategory.component';
import { Title } from '@angular/platform-browser';
import * as moment from "moment";
import { LocationModule, Location } from '../location/location.component';
import { Observable } from 'rxjs/Observable';

export class EventModule {
    public http: Http;
    public baseUrl: string;

    public find(e: Event): Observable<Event[]> {
        return this.http.post(this.baseUrl + 'api/event/find', {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            dateTimeEnd: e.dateTimeEnd,
            locationId: e.locationId
        }).map(response => response.json());
    }

    public add(e: Event): Observable<number> {
        return this.http.post(this.baseUrl + 'api/event/new', {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).map(response => response.json());
    }

    public edit(e: Event) {
        this.http.post(this.baseUrl + 'api/event/edit', {
            id: e.id,
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => { console.error(error); alert('Oops! Unknown error has occured.') });
    }
}

@Component({
    selector: 'event',
    templateUrl: './event.component.html'
})
export class EventComponent {
    private em: EventModule;

    public event: Event;
    public events: Event[];
    public eventCategories: EventCategory[];
    public locations: Location[];

    // https://stackoverflow.com/questions/44000162/how-to-change-title-of-a-page-using-angularangular-2-or-4-route
    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.em = new EventModule();
        this.em.http = http;
        this.em.baseUrl = baseUrl;
    }

    ngOnInit() {
        this.event = new Event();
        this.em.find(new Event()).subscribe(r => { this.events = r });

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
        e.dateTimeEnd = f.controls['dateTimeEnd'].value;
        e.locationId = f.controls['locationId'].value;

        this.em.find(e).subscribe(r => { this.events = r });
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

        this.em.add(e).subscribe(
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

    public onEditSubmit(event: Event) {
        var date = <HTMLInputElement>document.getElementById("txtDate");
        event.dateTimeStart = date.value;

        let e: any = this.events.find(x => x.id == event.id);
        event.attendance = e.attendance;

        let ec: any = this.eventCategories.find(x => x.id == event.eventCategoryId);
        event.category = ec.name;

        let l: any = this.locations.find(x => x.id == event.locationId);
        event.location = l.name;

        this.em.edit(event);

        for (let e of this.events) {
            if (e.id == event.id) {
                let index: number = this.events.indexOf(e);
                this.events[index] = event;
            }
        }
    }

    onDeleteClick() {
        var flag = confirm('Are you sure you want to delete selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        let options : any = {};
        options.url = "api/event/delete";
        options.type = "POST";
        options.data = JSON.stringify(selectedIds);
        options.contentType = "application/json";
        options.dataType = "json";
        options.success = () => {

            for (let id of selectedIds) {
                for (let e of this.events) {
                    if (e.id == id) {
                        this.events.splice(this.events.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');
        };
        options.error = function () {
            alert("Error while deleting the records!");
        };
        $.ajax(options);
    }
}

export class Event {
    id: number;
    eventCategoryId: number;
    category: string;
    name: string;
    dateTimeStart: string;
    dateTimeEnd: string;
    time: string;
    locationId: number;
    location: string;
    attendance: number;
}
