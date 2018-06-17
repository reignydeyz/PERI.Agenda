import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { Statistics, GraphDataSet, GraphData } from '../graph/graph.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

import { saveAs } from 'file-saver';

export class EventCategoryModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public find(ec: EventCategory): Observable<EventCategory[]> {
        return this.http.post(this.baseUrl + 'api/eventcategory/find', {
            name: ec.name
        }).map(response => response.json());
    }

    public add(ec: EventCategory): Observable<number> {
        return this.http.post(this.baseUrl + 'api/eventcategory/new', {
            name: ec.name
        }).map(response => response.json());
    }

    public edit(ec: EventCategory) {
        return this.http.post(this.baseUrl + 'api/eventcategory/edit', {
            id: ec.id,
            name: ec.name
        }).subscribe(result => { alert('Updated!'); $('#modalEdit').modal('toggle'); }, error => this.ex.catchError(error));
    }

    public get(id: number): Observable<EventCategory> {
        return this.http.get(this.baseUrl + 'api/eventcategory/get/' + id).map(response => response.json());
    }
}

@Component({
    selector: 'eventcategory',
    templateUrl: './eventcategory.component.html',
    styleUrls: ['./eventcategory.component.css',
        '../table/table.component.css']
})
export class EventCategoryComponent {
    private ecm: EventCategoryModule;

    public eventcategory = EventCategory;
    public eventcategories: EventCategory[];

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
        this.ecm = new EventCategoryModule();
        this.ecm.http = http;
        this.ecm.baseUrl = baseUrl;
        this.ecm.ex = new ErrorExceptionModule();
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngOnInit() {
        this.titleService.setTitle('Event Categories');
        this.ecm.find(new EventCategory()).subscribe(result => { this.eventcategories = result }, error => this.ecm.ex.catchError(error));        
    }

    ngAfterViewChecked() {
        if (this.eventcategories) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("table");
            tbl.onscroll = function () {
                $("table > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }
    }

    onNewSubmit(f: NgForm) {
        var ec = new EventCategory();
        ec.name = f.controls['name'].value;

        this.ecm.add(ec).subscribe(
            result => {
                ec.id = result;
                this.eventcategories.push(ec);

                alert('Added!');
                $('#modalNew').modal('toggle');
            },
            error => this.ecm.ex.catchError(error));
    }

    public onEditInit(id: number) {
        this.http.get(this.baseUrl + 'api/eventcategory/get/' + id)
            .subscribe(result => {
                this.eventcategory = result.json();
            }, error => this.ecm.ex.catchError(error));
    }

    public onEditSubmit(ec: EventCategory) {
        this.ecm.edit(ec);

        for (let e of this.eventcategories) {
            if (e.id == ec.id) {
                let index: number = this.eventcategories.indexOf(e);
                this.eventcategories[index] = ec;
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

        this.http.post(this.baseUrl + 'api/eventcategory/delete', body, options).subscribe(result => {

            for (let id of selectedIds) {
                for (let e of this.eventcategories) {
                    if (e.id == id) {
                        this.eventcategories.splice(this.eventcategories.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');

        }, error => this.ecm.ex.catchError(error));
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    onDownloadClick() {
        this.http.get(this.baseUrl + 'api/eventcategory/download').subscribe(result => {
            let parsedResponse = result.text();
            this.downloadFile(parsedResponse);
        }, error => this.ecm.ex.catchError(error));
    }

    onStatsLoad(id: number) {
        this.onEditInit(id);

        // Updating lineChartLabels does not reflect on the chart
        // https://github.com/valor-software/ng2-charts/issues/774
        this.chartLabels.length = 0;

        this.http.get(this.baseUrl + 'api/eventcategory/stats/' + id).subscribe(result => {
            this.stats = result.json() as GraphDataSet;
            this.chartLabels = this.stats.chartLabels;
        }, error => console.error(error));
    }
}

export class EventCategory {
    id: number;
    name: string;
    events: number;
    minAttendees: number;
    averageAttendees: number;
    maxAttendees: number;
}