import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { EventModule, Event } from '../event/event.component';
import { Title } from '@angular/platform-browser';

// https://angular-2-training-book.rangle.io/handout/routing/routeparams.html

@Component({
    selector: 'attendance',
    templateUrl: './attendance.component.html'
})
export class AttendanceComponent {
    id: number;
    event: Event;

    private sub: any;

    constructor(private route: ActivatedRoute, private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) { }

    ngOnInit() {
        this.sub = this.route.params.subscribe(params => {
            this.id = +params['id']; // (+) converts string 'id' to a number

            // In a real app: dispatch action to load the details here.
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    ngAfterViewInit() {
        var em = new EventModule();
        em.http = this.http;
        em.baseUrl = this.baseUrl;
        em.get(this.id).subscribe(r => { this.event = r });
    }
}
