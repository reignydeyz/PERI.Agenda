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
import { IMyDpOptions } from 'mydatepicker';

import { Pager } from '../pager/pager.component';
import { MemberModule, Member } from '../member/member.component';
import { Rsvp, RsvpModule } from '../rsvp/rsvp.component';
import { LookUp, LookUpModule } from '../lookup/lookup.component';

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

    public searchGoing(r: Rsvp): Observable<Rsvp[]> {
        return this.http.post(this.baseUrl + 'api/rsvp/find', {
            eventid: r.eventId,
            member: r.member,
            isGoing: r.isGoing
        }).map(r => r.json());
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
    styleUrls: ['./attendance.component.css',
        '../table/table.component.css'
    ]
})
export class AttendanceComponent {
    private rm: RsvpModule;
    private am: AttendanceModule;
    private mm: MemberModule;

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

    public searchAttendees: string;
    public pagerAttendees: Pager;
    public chunkAttendees: ChunkAttendees;

    public going: Rsvp[];

    public genders: LookUp[];

    private sub: any;

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };

    constructor(private route: ActivatedRoute, private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.am = new AttendanceModule();
        this.am.http = http;
        this.am.baseUrl = baseUrl;

        this.am.ex = new ErrorExceptionModule();
        this.am.ex.baseUrl = this.baseUrl;

        this.mm = new MemberModule();
        this.mm.http = http;
        this.mm.baseUrl = baseUrl;

        this.rm = new RsvpModule();
        this.rm.http = http;
        this.rm.baseUrl = baseUrl;
    }

    private paginate(obj: string, page: number) {
        let body = JSON.stringify(obj);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/attendance/' + this.id + '/search/page/' + page, body, options).subscribe(result => {
            this.chunk = result.json() as Chunk;
        }, error => this.am.ex.catchError(error));
    }

    private paginateAttendees(page: number) {
        this.http.get(this.baseUrl + 'api/attendance/' + this.id + '/attendees/page/' + page).subscribe(result => {
            this.chunkAttendees = result.json() as ChunkAttendees;
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

        this.pagerAttendees = new Pager();
    }

    ngOnDestroy() {
        this.sub.unsubscribe();

        let div: any;
        div = document.getElementsByClassName("col-sm-9 body-content");

        if (window.innerWidth >= 768) {
            div[0].style.width = "75%";
            $('#sidebar').show();
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
        var lm = new LookUpModule();
        lm.http = this.http;
        lm.baseUrl = this.baseUrl;
        lm.getByGroup('Gender').subscribe(result => { this.genders = result });

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

        this.mm.allNames().subscribe(result => { this.names = result });
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

    public onPaginateAttendees(page: number) {
        this.paginateAttendees(page);
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
                //alert('Success');
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
                //alert('Success');
            }, error => this.am.ex.catchError(error));
        }        
    }

    public onNewSubmit(f: NgForm) {
        var middleInitial = (f.controls['middleInitial'].value == ''
            || f.controls['middleInitial'].value == null
            || f.controls['middleInitial'].value == undefined) ? ' ' : ' ' + f.controls['middleInitial'].value + '. ';
        var name = f.controls['firstName'].value.trim() + middleInitial + f.controls['lastName'].value.trim();

        var m = new Member();
        m.name = name;
        m.nickName = f.controls['nickName'].value;

        if (f.controls['birthDate'].value != null) {
            m.birthDate = f.controls['birthDate'].value.formatted;
        }
        else {
            m.birthDate = '';
        }

        m.email = f.controls['email'].value;
        m.address = f.controls['address'].value;
        m.mobile = f.controls['mobile'].value;
        m.gender = f.controls['gender'].value;
        m.invitedByMemberName = f.controls['invitedBy'].value;
        m.remarks = f.controls['remarks'].value;

        this.mm.add(m).subscribe(
            result => {
                m.id = result;

                let frm: any;
                frm = document.getElementById("frmNew");
                frm.reset();

                var a = new Attendance();
                a.memberId = m.id;
                this.am.add(this.id, a).subscribe(r => { alert('Added!'); });

                this.totalAttendees++;
            },
            error => this.am.ex.catchError(error));
    }

    ngAfterViewChecked() {
        if (this.chunkAttendees) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("table");
            tbl.onscroll = function () {
                $("table > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }
    }

    onGoingClick() {
        var r = new Rsvp();
        r.eventId = this.id;
        r.isGoing = true;

        this.am.searchGoing(r).subscribe(result => {
            this.going = result;
        }, error => this.am.ex.catchError(error));
    }

    onAttendedClick(r: Rsvp) {
        for (let r of this.going) {
            if (r.memberId == r.memberId) {
                var a = new Attendance();
                a.memberId = r.memberId;
                a.name = r.member;
                a.dateTimeLogged = '';
                this.toggle(a);

                this.going.splice(this.going.indexOf(r), 1);
            }
        }
    }

    public names: string[];
    suggestions: string[] = [];

    suggest(fc: any) {
        if (fc.value.length > 0) {
            this.suggestions = this.names
                .filter(c => c.startsWith(fc.value.toUpperCase()))
                .slice(0, 5);
        }
        else {
            this.suggestions.length = 0;
        }
    }

    suggestionSelect(f: NgForm, s: string) {
        this.suggestions.length = 0;

        f.controls['invitedBy'].setValue(s);
    }
}

export class Attendance {
    memberId: number;
    name: string;
    dateTimeLogged: string;
}

class Chunk {
    registrants: Attendance[];
    pager: Pager;
}

class ChunkAttendees {
    attendees: Attendance[];
    pager: Pager;
}