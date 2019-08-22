import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { Title } from '@angular/platform-browser';

import { Statistics, GraphDataSet } from '../graph/graph.component';
import { ErrorExceptionModule } from '../errorexception/errorexception.component';
import { DashboardService } from '../../services/dashboard.service';
import { AccountService } from '../../services/account.service';

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

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title,
    private dashboardService: DashboardService, private am: AccountService) {
    }

    ngOnInit() {
        this.titleService.setTitle('Dashboard'); 
    }

    async ngAfterViewInit() {
        var r = await this.am.getRole();

        if (r.name != "Admin") {
            window.location.replace(this.baseUrl + 'calendar');
        }
        else {
            this.onAttendanceLoad();
        }
    }

    async onAttendanceLoad() {
        this.eventcategoryStats = await this.dashboardService.eventcategoryStats();
        this.locationStats = await this.dashboardService.locationStats();
    }

    async onMemberLoad() {
        this.memberStats = await this.dashboardService.memberStats();
    }

    async onGroupsLoad() {
        this.groupCategoryStats = await this.dashboardService.groupCategoryStats();
    }
}