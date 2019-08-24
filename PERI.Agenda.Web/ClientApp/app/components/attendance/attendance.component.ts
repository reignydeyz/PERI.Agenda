import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment';
import * as $ from "jquery";
import { IMyDpOptions } from 'mydatepicker';

import { Pager } from '../pager/pager.component';
import { RsvpModule } from '../rsvp/rsvp.component';
import { LookUp, LookUpModule } from '../lookup/lookup.component';

import { saveAs } from 'file-saver';

import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';  
import { MemberService } from '../../services/member.service';
import { Member } from '../../models/member';
import { EventCategory } from '../../models/eventcategory';
import { Event } from '../../models/event';
import { EventCategoryService } from '../../services/eventcategory.service';
import { EventService } from '../../services/event.service';
import { Attendance } from '../../models/attendance';
import { Rsvp } from '../../models/rsvp';
import { AttendanceService } from '../../services/attendance.service';

// https://angular-2-training-book.rangle.io/handout/routing/routeparams.html

@Component({
    selector: 'attendance',
    templateUrl: './attendance.component.html',
    styleUrls: ['./attendance.component.css',
        '../table/table.component.css'
    ]
})
export class AttendanceComponent {
    showLoader: boolean = true;
    private rm: RsvpModule;

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

    public firstTimers: Attendance[];

    private sub: any;
    private _hubConnection: HubConnection;  

    public myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'mm/dd/yyyy',
    };

    constructor(private route: ActivatedRoute, private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
   private mm: MemberService, private ecm: EventCategoryService, private em: EventService, private ex: ErrorExceptionModule, private am: AttendanceService) {
        
        this.rm = new RsvpModule();
        this.rm.http = http;
        this.rm.baseUrl = baseUrl;

        this.createConnection(); 
        this.startConnection(); 
        this.registerOnServerEvents();
    }

    private paginate(obj: string, page: number) {
        this.showLoader = true;

        this.am.search(this.id, obj, page).then(result => {
            this.chunk = result as Chunk;
            this.showLoader = false;
        }, error => this.ex.catchError(error));
    }

    private paginateAttendees(page: number) {
        this.am.paginateAttendees(this.id, page).then(result => {
            this.chunkAttendees = result as ChunkAttendees;
        }, error => this.ex.catchError(error));
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

        // Leave SignalRHub
        this.leaveGroup(this.id.toString());

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

    async ngAfterViewInit() {
        var lm = new LookUpModule();
        lm.http = this.http;
        lm.baseUrl = this.baseUrl;
        lm.getByGroup('Gender').subscribe(result => { this.genders = result });

        await this.em.get(this.id).then(r => {
            this.event = r;
            this.titleService.setTitle(r.name);
        }, error => this.ex.catchError(error));

        this.getTotal();

        let div: any;
        div = document.getElementsByClassName("col-sm-9 body-content");

        div[0].style.width = "100%";

        this.initMenu();

        this.names = await this.mm.allNames();
    }

    showMenuButton(): boolean {
        return window.innerWidth >= 768;
    }

    async onStatsLoad() {
        this.ec = await this.ecm.get(this.event.eventCategoryId);
    }

    private async getTotal() {
        await this.am.getTotal(this.id, 'all').then(result => {
            this.total = result;
        }, error => this.ex.catchError(error));

        await this.am.getTotal(this.id, 'attendees').then(result => {
            this.totalAttendees = result;
        }, error => this.ex.catchError(error));

        await this.am.getTotal(this.id, 'pending').then(result => {
            this.totalPending = result;
        }, error => this.ex.catchError(error));
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

    public onFirstTimers() {
        this.am.firstTimers(this.id).then(result => {
            this.firstTimers = result;
        }, error => this.ex.catchError(error));
    }

    public toggle(a: Attendance) {
        this.showLoader = true;
        if (a.dateTimeLogged != null && a.dateTimeLogged != '') {
            this.am.delete(this.id, a).then(r => {

                for (let r of this.chunk.registrants) {
                    if (r.memberId == a.memberId) {
                        let index: number = this.chunk.registrants.indexOf(r);

                        a.dateTimeLogged = '';
                        this.chunk.registrants[index] = a;
                    }
                }
                this.showLoader = false;
            }, error => this.ex.catchError(error));
        }
        else {
            this.am.add(this.id, a).then(r => {

                for (let r of this.chunk.registrants) {
                    if (r.memberId == a.memberId) {
                        let index: number = this.chunk.registrants.indexOf(r);

                        a.dateTimeLogged = moment().format('MM/DD/YYYY, h:mm:ss a');
                        this.chunk.registrants[index] = a;
                    }
                }
                this.showLoader = false;
            }, error => this.ex.catchError(error));
        }        
    }

    async onNewSubmit(f: NgForm) {
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

        m.id = await this.mm.add(m);

        let frm: any;
        frm = document.getElementById("frmNew");
        frm.reset();

        var a = new Attendance();
        a.memberId = m.id;
        this.am.add(this.id, a).then(r => { alert('Added!'); });

        this.totalAttendees++;
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

        if (this.firstTimers) {
            var tbl = <HTMLTableElement>document.getElementById("tbl2");
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

        this.am.searchGoing(r).then(result => {
            this.going = result;
        }, error => this.ex.catchError(error));
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

    sortByMemberName() {
        this.chunkAttendees.attendees.sort((a, b) => a.name.localeCompare(b.name));
    }

    sortByTimeLogged() {
        this.chunkAttendees.attendees.sort((a, b) => a.dateTimeLogged.localeCompare(b.dateTimeLogged));
    }

    sortByMemberNameFirstTimers() {
        this.firstTimers.sort((a, b) => a.name.localeCompare(b.name));
    }

    sortByTimeLoggedFirstTimers() {
        this.firstTimers.sort((a, b) => a.dateTimeLogged.localeCompare(b.dateTimeLogged));
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    downloadAttendees() {
        this.am.downloadAttendees(this.id).then(result => {
            let parsedResponse = result;
            this.downloadFile(parsedResponse);
        }, error => this.ex.catchError(error));;
    }

    downloadFirstTimers() {
        this.am.downloadFirstTimers(this.id).then(result => {
            let parsedResponse = result;
            this.downloadFile(parsedResponse);
        }, error => this.ex.catchError(error));;
    }

    // ============================================================
    // SignalR
    // https://www.c-sharpcorner.com/article/getting-started-with-signalr-using-aspnet-co-using-angular-5/
    private createConnection() {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl(this.baseUrl + 'AttendanceBroadcast')
            .build();
    }  

    private startConnection(): void {
        this._hubConnection
            .start()
            .then(() => {

                this.route.params.subscribe(params => {
                    this.joinGroup(params["id"]);
                });

                console.log('Hub connection started');
            })
            .catch(err => {
                console.log('Error while establishing connection, retrying...');
                console.log(err);
                setTimeout(this.startConnection(), 5000);
            });
    }

    private registerOnServerEvents(): void {
        this._hubConnection.on('AttendanceBroadcast', (data: any) => {

            for (let r of this.chunk.registrants) {
                if (r.memberId == data.memberId) {
                    let index: number = this.chunk.registrants.indexOf(r);

                    var a = this.chunk.registrants[index];

                    if (data.dateTimeLogged != null && data.dateTimeLogged != '' && data.dateTimeLogged != undefined) {
                        a.dateTimeLogged = moment(data.dateTimeLogged).format('MM/DD/YYYY, h:mm:ss a');
                        this.chunk.registrants[index] = a;

                        this.totalAttendees++;
                        this.totalPending--;
                    }
                    else {
                        a.dateTimeLogged = '';
                        this.chunk.registrants[index] = a;

                        this.totalAttendees--;
                        this.totalPending++;
                    }
                }
            }
        });
    }  

    // https://damienbod.com/2017/09/18/signalr-group-messages-with-ngrx-and-angular/
    joinGroup(group: string): void {
        if (this._hubConnection) {
            this._hubConnection.invoke('JoinGroup', group);
        }
    }

    leaveGroup(group: string): void {
        if (this._hubConnection) {
            this._hubConnection.invoke('LeaveGroup', group);
        }
    }
    // ============================================================
}

class Chunk {
    registrants: Attendance[];
    pager: Pager;
}

class ChunkAttendees {
    attendees: Attendance[];
    pager: Pager;
}