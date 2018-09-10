import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { EventCategoryModule, EventCategory } from '../eventcategory/eventcategory.component';
import { Title } from '@angular/platform-browser';
import * as moment from "moment";
import { LocationModule, Location } from '../location/location.component';
import { Observable } from 'rxjs/Observable';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Pager } from '../pager/pager.component';
import { saveAs } from 'file-saver';

export class EventModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

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

    public addExclusive(e: Event, groupId: number): Observable<number> {
        return this.http.post(this.baseUrl + 'api/event/new/exclusive/' + groupId, {
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        }).map(response => response.json());
    }

    public edit(e: Event) {
        return this.http.post(this.baseUrl + 'api/event/edit', {
            id: e.id,
            name: e.name,
            eventCategoryId: e.eventCategoryId,
            dateTimeStart: e.dateTimeStart,
            locationId: e.locationId
        });
    }

    public get(id: number): Observable<Event> {
        return this.http.get(this.baseUrl + 'api/event/get/' + id)
            .map(response => response.json());
    }
}

@Component({
    selector: 'event',
    templateUrl: './event.component.html',
    styleUrls: ['./event.component.css',
        '../table/table.component.css'
    ]
})
export class EventComponent {
    private em: EventModule;

    public event: Event;
    public eventCategories: EventCategory[];
    public locations: Location[];

    public search: Event;
    public pager: Pager;
    public chunk: Chunk;

    @Input('isAdmin') isAdmin: boolean = true;

    // https://stackoverflow.com/questions/44000162/how-to-change-title-of-a-page-using-angularangular-2-or-4-route
    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.em = new EventModule();
        this.em.http = http;
        this.em.baseUrl = baseUrl;

        this.em.ex = new ErrorExceptionModule();
        this.em.ex.baseUrl = this.baseUrl;
    }

    private paginate(obj: Event, page: number) {
        if (this.isAdmin) {
            this.http.post(this.baseUrl + 'api/event/find/page/' + page, obj).subscribe(result => {
                this.chunk = result.json() as Chunk;
            }, error => this.em.ex.catchError(error));
        }
        else {
            this.http.post(this.baseUrl + 'api/event/find/mypage/' + page, obj).subscribe(result => {
                this.chunk = result.json() as Chunk;
            }, error => this.em.ex.catchError(error));
        }
    }

    ngOnInit() {
        this.event = new Event();
        this.search = new Event();
        this.pager = new Pager();
        this.paginate(this.event, 1);

        this.titleService.setTitle('Events');

        var ecc = new EventCategoryModule();
        ecc.http = this.http;
        ecc.baseUrl = this.baseUrl;
        ecc.ex = this.em.ex;
        ecc.find(new EventCategory()).subscribe(result => { this.eventCategories = result });

        var l = new LocationModule();
        l.http = this.http;
        l.baseUrl = this.baseUrl;
        l.find(new Location()).subscribe(result => { this.locations = result });       
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngAfterViewChecked() {
        if (this.chunk) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("table");
            tbl.onscroll = function () {
                $("table > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }
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

        this.paginate(e, 1);
        this.search = e;
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    public onPaginate(page: number) {
        this.paginate(this.search, page);
    }

    public onDownloadClick() {
        this.download(this.search);
    }

    private download(e: Event) {
        this.http.post(this.baseUrl + 'api/event/download', e).subscribe(result => {
            let parsedResponse = result.text();
            this.downloadFile(parsedResponse);
        }, error => this.em.ex.catchError(error));;
    }

    onEventAdd(eventId: number) {
        if (eventId > 0) {
            // Get event
            this.http.get(this.baseUrl + 'api/event/get/' + eventId)
                .subscribe(result => {
                    var g = result.json();

                    // Add new event to the list
                    //this.chunk.events.push(g);
                    this.chunk.events.splice(0, 0, g);
                    this.chunk.pager.totalItems++;

                }, error => this.em.ex.catchError(error));
        }
    }

    public onEditInit(id: number) {
        this.http.get(this.baseUrl + 'api/event/get/' + id)
            .subscribe(result => { this.event = result.json() as Event }, error => this.em.ex.catchError(error));
    }

    onEventInfoChange(eventId: number) {
        if (eventId > 0) {
            // Get event
            this.http.get(this.baseUrl + 'api/event/get/' + eventId)
                .subscribe(result => {
                    var res = result.json();

                    for (let e of this.chunk.events) {
                        if (e.id == res.id) {
                            let index: number = this.chunk.events.indexOf(e);
                            this.chunk.events[index] = res;
                        }
                    }

                }, error => this.em.ex.catchError(error));
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

        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/event/delete', body, options).subscribe(result => {

            for (let id of selectedIds) {
                for (let e of this.chunk.events) {
                    if (e.id == id) {
                        this.chunk.events.splice(this.chunk.events.indexOf(e), 1);

                        this.chunk.pager.totalItems--;
                    }
                }
            }

            alert('Success!');

        }, error => this.em.ex.catchError(error));
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
    isExclusive: boolean;
}

class Chunk {
    events: Event[];
    pager: Pager;
}