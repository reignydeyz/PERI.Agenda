import { Component, Inject, AfterViewInit, Input } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import { Title } from '@angular/platform-browser';
import * as moment from "moment";
import { Observable } from 'rxjs/Observable';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Pager } from '../pager/pager.component';
import { saveAs } from 'file-saver';
import { EventCategory } from '../../models/eventcategory';
import { EventCategoryService } from '../../services/eventcategory.service';
import { LocationService } from '../../services/location.service';
import { Location } from '../../models/location';
import { Event } from '../../models/event';
import { EventService } from '../../services/event.service';

@Component({
    selector: 'event',
    templateUrl: './event.component.html',
    styleUrls: ['./event.component.css',
        '../table/table.component.css'
    ]
})
export class EventComponent {
    public event: Event;
    public eventCategories: EventCategory[];
    public locations: Location[];

    public search: Event;
    public pager: Pager;
    public chunk: Chunk;

    @Input('isAdmin') isAdmin: boolean = true;

    // https://stackoverflow.com/questions/44000162/how-to-change-title-of-a-page-using-angularangular-2-or-4-route
    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title, private ecm: EventCategoryService,
    private lm: LocationService, private em: EventService, private ex: ErrorExceptionModule) {
    }

    private async paginate(obj: Event, page: number) {
        if (this.isAdmin) {
            await this.em.search(obj, page).then(result => {
                this.chunk = result.json() as Chunk;
            }, error => this.ex.catchError(error));
        }
        else {
            await this.em.searchMyPage(obj, page).then(result => {
                this.chunk = result.json() as Chunk;
            }, error => this.ex.catchError(error));
        }
    }

    async ngOnInit() {
        this.event = new Event();
        this.search = new Event();
        this.pager = new Pager();
        this.paginate(this.event, 1);

        if (this.isAdmin) {
            this.titleService.setTitle('Events');
        }
        else {
            this.titleService.setTitle('My Agenda - Events');
        }

        this.eventCategories = await this.ecm.find(new EventCategory());
        this.locations = await this.lm.find(new Location());
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

    private async download(e: Event) {
        await this.em.download(e).then(result => {
            let parsedResponse = result;
            this.downloadFile(parsedResponse);
        }, error => this.ex.catchError(error));;
    }

    async onEventAdd(eventId: number) {
        if (eventId > 0) {
            // Get event
            await this.em.get(eventId)
                .then(result => {
                    var g = result;

                    // Add new event to the list
                    //this.chunk.events.push(g);
                    this.chunk.events.splice(0, 0, g);
                    this.chunk.pager.totalItems++;

                }, error => this.ex.catchError(error));
        }
    }

    async onEditInit(id: number) {
        this.event = await this.em.get(id);
    }

    async onEventInfoChange(eventId: number) {
        if (eventId > 0) {
            // Get event
            await this.em.get(eventId)
                .then(result => {
                    var res = result;

                    for (let e of this.chunk.events) {
                        if (e.id == res.id) {
                            let index: number = this.chunk.events.indexOf(e);
                            this.chunk.events[index] = res;
                        }
                    }

                }, error => this.ex.catchError(error));
        }
    }

    async onDeleteClick() {
        var flag = confirm('Are you sure you want to delete selected records?');

        if (!flag)
            return false;

        var selectedIds = new Array();
        $('input:checkbox.checkBox').each(function () {
            if ($(this).prop('checked')) {
                selectedIds.push($(this).val());
            }
        });

        await this.em.delete(selectedIds).then(() => {

            for (let id of selectedIds) {
                for (let e of this.chunk.events) {
                    if (e.id == id) {
                        this.chunk.events.splice(this.chunk.events.indexOf(e), 1);

                        this.chunk.pager.totalItems--;
                    }
                }
            }

            alert('Success!');

        }, error => this.ex.catchError(error));
    }
}

class Chunk {
    events: Event[];
    pager: Pager;
}