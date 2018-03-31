import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { Title } from '@angular/platform-browser';

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

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
    }

    ngOnInit() {
        this.titleService.setTitle('Dashboard'); 
    }

    ngAfterViewInit() {
        this.onAttendanceLoad();
    }

    onAttendanceLoad() {
        this.http.get(this.baseUrl + 'api/dashboard/eventcategories').subscribe(result => {
            this.eventcategoryStats = result.json() as Statistics;
        }, error => console.error(error));

        this.http.get(this.baseUrl + 'api/dashboard/locations').subscribe(result => {
            this.locationStats = result.json() as Statistics;
        }, error => console.error(error));
    }

    onMemberLoad() {
        this.http.get(this.baseUrl + 'api/dashboard/member').subscribe(result => {
            this.memberStats = result.json() as Statistics;
        }, error => console.error(error));
    }

    onGroupsLoad() {
        this.http.get(this.baseUrl + 'api/dashboard/groupcategories').subscribe(result => {
            this.groupCategoryStats = result.json() as GraphDataSet; console.log(result.json());
        }, error => console.error(error));
    }
}

interface Statistics {
    labels: Array<string>;
    values: Array<number>;
}

interface GraphData {
    data: Array<number>;
    label: string;
}

interface GraphDataSet {
    dataSet: GraphData[];
    chartLabels: string[];
}