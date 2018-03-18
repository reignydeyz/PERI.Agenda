import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'dashboard',
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent {

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {

    }

    ngOnInit() {
        this.titleService.setTitle('Dashboard'); 
    }
}