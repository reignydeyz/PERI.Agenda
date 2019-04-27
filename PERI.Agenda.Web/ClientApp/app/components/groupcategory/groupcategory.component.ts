import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Group } from '../group/group.component';
import { Statistics, GraphDataSet, GraphData } from '../graph/graph.component';
import { saveAs } from 'file-saver';

export class GroupCategoryModule {
    public http: Http;
    public baseUrl: string;

    public ex: ErrorExceptionModule;

    public find(ec: GroupCategory): Observable<GroupCategory[]> {
        return this.http.post(this.baseUrl + 'api/groupcategory/find', {
            name: ec.name
        }).map(response => response.json());
    }

    public add(gc: GroupCategory): Observable<number> {
        return this.http.post(this.baseUrl + 'api/groupcategory/new', {
            name: gc.name
        }).map(response => response.json());
    }

    public edit(gc: GroupCategory): Observable<number> {
        return this.http.post(this.baseUrl + 'api/groupcategory/edit', {
            id: gc.id,
            name: gc.name
        }).map(response => response.json());
    }
}

@Component({
    selector: 'groupcategory',
    templateUrl: './groupcategory.component.html',
    styleUrls: ['./groupcategory.component.css',
        '../table/table.component.css']
})
export class GroupCategoryComponent {
    private gcm: GroupCategoryModule;

    public groupcategory = GroupCategory;
    public groupcategories: GroupCategory[];

    groups: Group[];

    public stats: GraphDataSet;
    public chartLabels: string[] = [];
    public chartType: string = 'pie';
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
        this.gcm = new GroupCategoryModule();
        this.gcm.http = http;
        this.gcm.baseUrl = baseUrl;

        this.gcm.ex = new ErrorExceptionModule();
        this.gcm.ex.baseUrl = this.baseUrl;
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    download(id: number) {
        this.http.get(this.baseUrl + 'api/groupcategory/' + id + '/download').subscribe(result => {
            let parsedResponse = result.text();
            this.downloadFile(parsedResponse);
        }, error => this.gcm.ex.catchError(error));;
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    ngOnInit() {
        this.titleService.setTitle('Group Categories');

        this.gcm.find(new GroupCategory()).subscribe(result => { this.groupcategories = result });
    }

    ngAfterViewChecked() {
        if (this.groupcategories) {
            var tbl = <HTMLTableElement>document.getElementById("tbl");
            let tbl1: any;
            tbl1 = $("table");
            tbl.onscroll = function () {
                $("table > *").width(tbl1.width() + tbl1.scrollLeft());
            };
        }
    }

    onNewSubmit(f: NgForm) {
        var gc = new GroupCategory();
        gc.name = f.controls['name'].value;

        this.gcm.add(gc).subscribe(
            result => {
                gc.id = result;
                this.groupcategories.push(gc);

                alert('Added!');
                $('#modalNew').modal('toggle');
            },
            error => this.gcm.ex.catchError(error));
    }

    onEditInit(id: number) {
        this.http.get(this.baseUrl + 'api/groupcategory/get/' + id)
            .subscribe(result => {
                console.log(result.json());
                this.groupcategory = result.json();
            }, error => this.gcm.ex.catchError(error));
    }

    public onEditSubmit(gc: GroupCategory) {
        this.gcm.edit(gc).subscribe(r => {

            this.onGroupCategoryInfoChange(gc.id);

            alert('Updated!');
            $('#modalEdit').modal('toggle');
        });
    }

    onGroupCategoryInfoChange(gcId: number) {
        if (gcId > 0) {
            // Get group category
            this.http.get(this.baseUrl + 'api/groupcategory/get/' + gcId)
                .subscribe(result => {
                    var res = result.json();

                    for (let e of this.groupcategories) {
                        if (e.id == res.id) {
                            let index: number = this.groupcategories.indexOf(e);
                            this.groupcategories[index] = res;
                        }
                    }

                }, error => this.gcm.ex.catchError(error));
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

        this.http.post(this.baseUrl + 'api/groupcategory/delete', body, options).subscribe(result => {

            for (let id of selectedIds) {
                for (let e of this.groupcategories) {
                    if (e.id == id) {
                        this.groupcategories.splice(this.groupcategories.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');

        }, error => this.gcm.ex.catchError(error));
    }

    onStatsLoad(id: number) {
        this.onEditInit(id);

        // Updating lineChartLabels does not reflect on the chart
        // https://github.com/valor-software/ng2-charts/issues/774
        this.chartLabels.length = 0;

        this.http.get(this.baseUrl + 'api/groupcategory/stats/' + id).subscribe(result => {
            this.stats = result.json() as GraphDataSet;
            this.chartLabels = this.stats.chartLabels;
        }, error => console.error(error));
    }
}

export class GroupCategory {
    id: number;
    name: string;
    groups: number;
}