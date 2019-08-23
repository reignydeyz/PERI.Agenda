import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import * as $ from "jquery";

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { Statistics, GraphDataSet, GraphData } from '../graph/graph.component';
import { saveAs } from 'file-saver';
import { Group } from '../../models/group';
import { GroupCategory } from '../../models/groupcategory';
import { GroupCategoryService } from '../../services/groupcategory.service';

@Component({
    selector: 'groupcategory',
    templateUrl: './groupcategory.component.html',
    styleUrls: ['./groupcategory.component.css',
        '../table/table.component.css']
})
export class GroupCategoryComponent {
    public groupcategory: GroupCategory = new GroupCategory();
    public groupcategories: GroupCategory[] = [];

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

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private gcm: GroupCategoryService, private ex: ErrorExceptionModule) {
    }

    downloadFile(data: any) {
        var blob = new Blob([data], { type: 'text/csv' });
        saveAs(blob, "data.csv");
    }

    async download(id: number) {
        this.downloadFile(await this.gcm.download(id));
    }

    checkAll() {
        var src = <HTMLInputElement>document.getElementById("checkall");

        $("#tbl").find('input[type=checkbox]').each(function () {
            var element = <HTMLInputElement>this;
            element.checked = src.checked;
        });
    }

    async ngOnInit() {
        this.titleService.setTitle('Group Categories');

        this.groupcategories = await this.gcm.find(new GroupCategory());
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

    async onNewSubmit(f: NgForm) {
        var gc = new GroupCategory();
        gc.name = f.controls['name'].value;

        await this.gcm.add(gc).then(r => {
            gc.id = r;
            this.groupcategories.push(gc);

            alert('Added!');
            $('#modalNew').modal('toggle');
        }).catch(err => {
            this.ex.catchError(err);
        });
    }

    async onEditInit(id: number) {
        this.groupcategory = await this.gcm.get(id);
    }

    async onEditSubmit(gc: GroupCategory) {
        await this.gcm.edit(gc).then(() => {
            this.onGroupCategoryInfoChange(gc.id);

            alert('Updated!');
            $('#modalEdit').modal('toggle');
        });
    }

    async onGroupCategoryInfoChange(gcId: number) {
        if (gcId > 0) {
            var res = await this.gcm.get(gcId);

            for (let e of this.groupcategories) {
                if (e.id == res.id) {
                    let index: number = this.groupcategories.indexOf(e);
                    this.groupcategories[index] = res;
                }
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

        await this.gcm.delete(selectedIds).then(() => {
            for (let id of selectedIds) {
                for (let e of this.groupcategories) {
                    if (e.id == id) {
                        this.groupcategories.splice(this.groupcategories.indexOf(e), 1);
                    }
                }
            }

            alert('Success!');
        });
    }

    async onStatsLoad(id: number) {
        this.onEditInit(id);

        // Updating lineChartLabels does not reflect on the chart
        // https://github.com/valor-software/ng2-charts/issues/774
        this.chartLabels.length = 0;

        this.stats = await this.gcm.stats(id);
        this.chartLabels = this.stats.chartLabels;
    }
}