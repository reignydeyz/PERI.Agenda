import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { Statistics, GraphDataSet, GraphData } from '../graph/graph.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Event } from '../../models/event';
import { saveAs } from 'file-saver';
import { EventCategory } from '../../models/eventcategory';
import { EventCategoryService } from '../../services/eventcategory.service';

@Component({
    selector: 'eventcategory',
    templateUrl: './eventcategory.component.html',
    styleUrls: ['./eventcategory.component.css',
        '../table/table.component.css']
})
export class EventCategoryComponent {
    public eventcategory: EventCategory = new EventCategory();
    public eventcategories: EventCategory[];
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
    private ecm: EventCategoryService, private ex: ErrorExceptionModule) {
        
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    async ngOnInit() {
        this.titleService.setTitle('Event Categories');
        this.eventcategories = await this.ecm.find(new EventCategory());
    }

    ngAfterViewChecked() {
        if (this.eventcategories) {
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

    async onNewSubmit(f: NgForm) {
        var ec = new EventCategory();
        ec.name = f.controls['name'].value;

        await this.ecm.add(ec).then(result => {
            ec.id = result;
            this.eventcategories.push(ec);

            alert('Added!');
            $('#modalNew').modal('toggle');
        }).catch(error => {
            this.ex.catchError(error);
        });
    }

    async onEditInit(id: number) {
        this.eventcategory = await this.ecm.get(id);
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

        await this.ecm.delete(selectedIds).then(() => {
            for (let id of selectedIds) {
                for (let e of this.eventcategories) {
                    if (e.id == id) {
                        this.eventcategories.splice(this.eventcategories.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');
        }).catch(error => {
            this.ex.catchError(error);
        });
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    async onDownloadClick() {
        this.downloadFile(await this.ecm.download());
    }

    async onStatsLoad(id: number) {
        await this.onEditInit(id);

        // Updating lineChartLabels does not reflect on the chart
        // https://github.com/valor-software/ng2-charts/issues/774
        this.chartLabels.length = 0;

        await this.ecm.stats(id).then(result => {
            this.stats = result;
            this.chartLabels = this.stats.chartLabels;
        }).catch(error => {
            this.ex.catchError(error);
        });
    }

    async onEventsLoad(id: number) {
        this.events = await this.ecm.events(id);
    }
}