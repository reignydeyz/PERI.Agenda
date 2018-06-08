import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { EventCategoryModule, EventCategory } from '../eventcategory/eventcategory.component';
import { EventModule, Event } from '../event/event.component';
import { Title } from '@angular/platform-browser';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment';
import * as $ from "jquery";

import { Pager } from '../pager/pager.component';

export class AttendanceModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public registrants(eventId: number): Observable<Attendance[]> {
        return this.http.get(this.baseUrl + 'api/attendance/' + eventId)
            .map(r => r.json());
    }

    public searchRegistrants(eventId: number, member: string): Observable<Attendance[]> {
        let body = JSON.stringify(member);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this.baseUrl + 'api/attendance/' + eventId + '/search', body, options)
        .map(r => r.json());
    }

    public add(eventId: number, a: Attendance): Observable<number> {
        return this.http.put(this.baseUrl + 'api/attendance/' + eventId + '/add', {
            memberId: a.memberId,
            dateTimeLogged: moment().format('MM/DD/YYYY, h:mm:ss a')
        }).map(r => r.json());
    }

    public delete(eventId: number, a: Attendance) {
        return this.http.post(this.baseUrl + 'api/attendance/' + eventId + '/delete', {
            memberId: a.memberId
        });
    }
}

// https://angular-2-training-book.rangle.io/handout/routing/routeparams.html

@Component({
    selector: 'attendance',
    templateUrl: './attendance.component.html',
    styleUrls: ['./attendance.component.css']
})
export class AttendanceComponent {
    private am: AttendanceModule;

    id: number;
    event: Event;
    //registrants: Attendance[];

    public ec: EventCategory;

    public total: number;
    public totalAttendees: number;
    public totalPending: number;

    public search: string;
    public pager: Pager;
    public chunk: Chunk;

    private sub: any;

    constructor(private route: ActivatedRoute, private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.am = new AttendanceModule();
        this.am.http = http;
        this.am.baseUrl = baseUrl;

        this.am.ex = new ErrorExceptionModule();
        this.am.ex.baseUrl = this.baseUrl;
    }

    private paginate(obj: string, page: number) {
        let body = JSON.stringify(obj);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/attendance/' + this.id + '/search/page/' + page, body, options).subscribe(result => {
            this.chunk = result.json() as Chunk;
        }, error => this.am.ex.catchError(error));
    }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id']; // (+) converts string 'id' to a number

            // In a real app: dispatch action to load the details here.
        });

        this.search = "";
        this.pager = new Pager();
        this.paginate(this.search, 1);
    }

    ngOnDestroy() {
        this.sub.unsubscribe();

        let div: any;
        div = document.getElementsByClassName("col-sm-9 body-content");

        if (window.innerWidth >= 768) {
            div[0].style.width = "75%";
        }
    }

    toggleMenu() {
        $('#sidebar').toggle();
    }

    initMenu() {
        if (window.innerWidth >= 768) {
            $('#sidebar').hide();
            $('#sidebarCollapse').show();
        }
        else {
            $('#sidebar').show();
            $('#sidebarCollapse').hide();
        }
    }

    ngAfterViewInit() {
        var em = new EventModule();
        em.http = this.http;
        em.baseUrl = this.baseUrl;

        em.ex = new ErrorExceptionModule();
        em.ex.baseUrl = this.baseUrl;

        em.get(this.id).subscribe(r => {
            this.event = r;
            this.titleService.setTitle(r.name);
        }, error => em.ex.catchError(error));

        this.getTotal();

        let div: any;
        div = document.getElementsByClassName("col-sm-9 body-content");

        div[0].style.width = "100%";

        this.initMenu();
    }

    showMenuButton(): boolean {
        return window.innerWidth >= 768;
    }

    onStatsLoad() {
        var ecm = new EventCategoryModule();
        ecm.http = this.http;
        ecm.baseUrl = this.baseUrl;
        ecm.ex = new ErrorExceptionModule();
        ecm.get(this.event.eventCategoryId).subscribe(res => { this.ec = res }, error => ecm.ex.catchError(error));
    }

    private getTotal() {
        var ex = new ErrorExceptionModule();

        this.http.get(this.baseUrl + 'api/attendance/' + this.id + '/total/all').subscribe(result => {
            this.total = result.json() as number;
        }, error => ex.catchError(error));

        this.http.get(this.baseUrl + 'api/attendance/' + this.id + '/total/attendees').subscribe(result => {
            this.totalAttendees = result.json() as number;
        }, error => ex.catchError(error));

        this.http.get(this.baseUrl + 'api/attendance/' + this.id + '/total/pending').subscribe(result => {
            this.totalPending = result.json() as number;
        }, error => ex.catchError(error));
    }

    public onSearchRegistrantsSubmit(f: NgForm) {
        /*this.am.searchRegistrants(this.id, f.controls['name'].value)
            .subscribe(r => { this.chunk.registrants = r }, error => this.am.ex.catchError(error));*/

        this.paginate(f.controls['name'].value, 1);
        this.search = f.controls['name'].value;
    }

    public onPaginate(page: number) {
        this.paginate(this.search, page);
    }

    public toggle(a: Attendance) {
        if (a.dateTimeLogged != null && a.dateTimeLogged != '') {
            this.am.delete(this.id, a).subscribe(r => {
                for (let r of this.chunk.registrants) {
                    if (r.memberId == a.memberId) {
                        let index: number = this.chunk.registrants.indexOf(r);

                        a.dateTimeLogged = '';
                        this.chunk.registrants[index] = a;
                    }
                }
                this.totalAttendees--;
                this.totalPending++;
                alert('Success');
            }, error => this.am.ex.catchError(error));
        }
        else {
            this.am.add(this.id, a).subscribe(r => {
                for (let r of this.chunk.registrants) {
                    if (r.memberId == a.memberId) {
                        let index: number = this.chunk.registrants.indexOf(r);

                        a.dateTimeLogged = moment().format('MM/DD/YYYY, h:mm:ss a');
                        this.chunk.registrants[index] = a;
                    }
                }
                this.totalAttendees++;
                this.totalPending--;
                alert('Success');
            }, error => this.am.ex.catchError(error));
        }        
    }
}

export class Attendance {
    memberId: number;
    member: string;
    dateTimeLogged: string;
}

class Chunk {
    registrants: Attendance[];
    pager: Pager;
}