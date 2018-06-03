﻿import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { Title } from '@angular/platform-browser';

import { Statistics, GraphDataSet } from '../graph/graph.component';
import { AccountModule, Role } from "../account/account.component";
import { ErrorExceptionModule } from '../errorexception/errorexception.component';

@Component({
    selector: 'dashboard',
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
    public memberStats: Statistics;
    public memberChartType: string = 'pie';
    
    public eventcategoryStats: Statistics;
    public eventcategoryChartType: string = 'pie';

    public locationStats: Statistics;
    public locationChartType: string = 'pie';

    public groupCategoryStats: GraphDataSet;
    public groupCategoryChartType: string = 'bar';
    public groupCategoryChartLegend: boolean = true;
    public groupCategoryChartOptions: any = {
        responsive: true,
        legend: {
            display: false,
            labels: {
                display: false
            }
        }
    };

    private am: AccountModule

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.am = new AccountModule();
        this.am.http = http;
        this.am.baseUrl = baseUrl;

        this.am.ex = new ErrorExceptionModule();
        this.am.ex.baseUrl = this.baseUrl;
    }

    ngOnInit() {
        this.titleService.setTitle('Dashboard'); 
    }

    ngAfterViewInit() {

        this.am.getRole().subscribe(r => {
            if (r.name != "Admin") {
                window.location.replace(this.baseUrl + 'calendar');
            }
            else {
                this.onAttendanceLoad();
            }
        }, error => this.am.ex.catchError(error));
    }

    onAttendanceLoad() {
        this.http.get(this.baseUrl + 'api/dashboard/eventcategories').subscribe(result => {
            this.eventcategoryStats = result.json() as Statistics;
        }, error => this.am.ex.catchError(error));

        this.http.get(this.baseUrl + 'api/dashboard/locations').subscribe(result => {
            this.locationStats = result.json() as Statistics;
        }, error => this.am.ex.catchError(error));
    }

    onMemberLoad() {
        this.http.get(this.baseUrl + 'api/dashboard/member').subscribe(result => {
            this.memberStats = result.json() as Statistics;
        }, error => this.am.ex.catchError(error));
    }

    onGroupsLoad() {
        this.http.get(this.baseUrl + 'api/dashboard/groupcategories').subscribe(result => {
            this.groupCategoryStats = result.json() as GraphDataSet; console.log(result.json());
        }, error => console.error(error));
    }
}