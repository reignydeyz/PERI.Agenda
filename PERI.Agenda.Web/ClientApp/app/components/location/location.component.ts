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
import { Location } from '../../models/location';
import { LocationService } from '../../services/location.service';

@Component({
    selector: 'location',
    templateUrl: './location.component.html',
    styleUrls: ['./location.component.css',
        '../table/table.component.css']
})
export class LocationComponent {
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

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private lm: LocationService, private ex: ErrorExceptionModule) {
        
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    async ngOnInit() {
        this.titleService.setTitle('Locations');
        this.locations = await this.lm.find(new Location());
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

    async onEditInit(id: number) {
        await this.lm.get(id)
            .then(result => {
                this.location = result;
            }, error => this.ex.catchError(error));
    }

    async onNewSubmit(f: NgForm) {
        var l = new Location();
        l.name = f.controls['name'].value;
        l.address = f.controls['address'].value;

        this.lm.add(l).then(
            result => {
                l.id = result;
                this.locations.push(l);

                alert('Added!');
                $('#modalNew').modal('toggle');
            },
            error => this.ex.catchError(error));
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

        await this.lm.delete(selectedIds).then(result => {

            for (let id of selectedIds) {
                for (let e of this.locations) {
                    if (e.id == id) {
                        this.locations.splice(this.locations.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');

        }, error => this.ex.catchError(error));
    }

    onStatsLoad(id: number) {
        this.onEditInit(id);

        // Updating lineChartLabels does not reflect on the chart
        // https://github.com/valor-software/ng2-charts/issues/774
        this.chartLabels.length = 0;

        this.lm.stats(id).then(result => {
            this.stats = result;
            this.chartLabels = this.stats.chartLabels;
        }, error => console.error(error));
    }

    async onEventsLoad(id: number) {
        this.lm.events(id).then(result => {
            this.events = result.json()
        }, error => console.error(error));
    }
}