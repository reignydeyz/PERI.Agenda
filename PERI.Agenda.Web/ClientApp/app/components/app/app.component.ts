import { Component, Inject } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    version: string = "";

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        
    }

    ngOnInit() {
        // Version
        this.http.get(this.baseUrl + 'version').subscribe(result => {
            this.version = result.json();
        });
    }
}
