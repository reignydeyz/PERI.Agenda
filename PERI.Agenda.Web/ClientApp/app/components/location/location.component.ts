import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { Statistics, GraphDataSet, GraphData } from '../graph/graph.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Event } from '../event/event.component';

export class LocationModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public add(l: Location): Observable<number> {
        return this.http.post(this.baseUrl + 'api/location/new', {
            name: l.name,
            address: l.address
        }).map(response => response.json());
    }

    public edit(l: Location) {
        return this.http.post(this.baseUrl + 'api/location/edit', {
            id: l.id,
            name: l.name,
            address: l.address
        }).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => this.ex.catchError(error));
    }

    public find(l: Location): Observable<Location[]> {
        return this.http.post(this.baseUrl + 'api/location/find', {
            name: l.name
        }).map(response => response.json());
    }
}

@Component({
    selector: 'location',
    templateUrl: './location.component.html',
    styleUrls: ['./location.component.css',
        '../table/table.component.css']
})
export class LocationComponent {
    private lm: LocationModule;
    location: Location = new Location(); 
    locations: Location[];
    events: Event[];

    public stats: GraphDataSet;
    public chartLabels: string[] = [];
    public chartType: string = 'line';
    public chartLegend: boolean = true;
    public chartOptions: any = {
        responsive: true,
        legend: {
            display: false,
            labels: {
                display: false
            }
        }
    };

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.lm = new LocationModule();
        this.lm.http = http;
        this.lm.baseUrl = baseUrl;
        this.lm.ex = new ErrorExceptionModule();
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngOnInit() {
        this.titleService.setTitle('Locations');
        this.lm.find(new Location()).subscribe(result => { this.locations = result }, error => this.lm.ex.catchError(error));
    }

    ngAfterViewChecked() {
        if (this.locations) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("#tbl");
            tbl.onscroll = function () {
                $("#tbl > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }

        if (this.events) {
            var tbl = <HTMLTableElement>document.getElementById("tbl1");
            if (tbl != null && tbl != undefined) {
                let tbl1: any;
                tbl1 = $("#tbl1");
                tbl.onscroll = function () {
                    $("#tbl1 > *").width(tbl1.width() + tbl1.scrollLeft());
                };
            }
        }
    }

    onEditInit(id: number) {
        this.http.get(this.baseUrl + 'api/location/get/' + id)
            .subscribe(result => {
                this.location = result.json();
            }, error => this.lm.ex.catchError(error));
    }

    onNewSubmit(f: NgForm) {
        var l = new Location();
        l.name = f.controls['name'].value;
        l.address = f.controls['address'].value;

        this.lm.add(l).subscribe(
            result => {
                l.id = result;
                this.locations.push(l);

                alert('Added!');
                $('#modalNew').modal('toggle');
            },
            error => this.lm.ex.catchError(error));
    }

    public onEditSubmit(l: Location) {
        this.lm.edit(l);

        for (let e of this.locations) {
            if (e.id == l.id) {
                let index: number = this.locations.indexOf(e);
                this.locations[index] = l;
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

        let body = JSON.stringify(selectedIds);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        this.http.post(this.baseUrl + 'api/location/delete', body, options).subscribe(result => {

            for (let id of selectedIds) {
                for (let e of this.locations) {
                    if (e.id == id) {
                        this.locations.splice(this.locations.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');

        }, error => this.lm.ex.catchError(error));
    }

    onStatsLoad(id: number) {
        this.onEditInit(id);

        // Updating lineChartLabels does not reflect on the chart
        // https://github.com/valor-software/ng2-charts/issues/774
        this.chartLabels.length = 0;

        this.http.get(this.baseUrl + 'api/location/stats/' + id).subscribe(result => {
            this.stats = result.json() as GraphDataSet;
            this.chartLabels = this.stats.chartLabels;
        }, error => console.error(error));
    }

    onEventsLoad(id: number) {
        this.http.get(this.baseUrl + 'api/location/events/' + id).subscribe(result => {
            this.events = result.json()
        }, error => console.error(error));
    }
}

export class Location {
    id: number;
    name: string;
    address: string;
    minAttendees: number;
    averageAttendees: number;
    maxAttendees: number;
}