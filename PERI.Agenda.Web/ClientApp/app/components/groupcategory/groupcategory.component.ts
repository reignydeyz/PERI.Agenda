import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';
import { Title } from '@angular/platform-browser';

export class GroupCategoryModule {
    public http: Http;
    public baseUrl: string;

    public find(ec: GroupCategory): Observable<GroupCategory[]> {
        return this.http.post(this.baseUrl + 'api/groupcategory/find', {
            name: ec.name
        }).map(response => response.json());
    }
}

@Component({
    selector: 'groupcategory',
    templateUrl: './groupcategory.component.html'
})
export class GroupCategoryComponent {
    private gcm: GroupCategoryModule;

    public groupcategory = GroupCategory;
    public groupcategories: GroupCategory[];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private titleService: Title) {
        this.gcm = new GroupCategoryModule();
        this.gcm.http = http;
        this.gcm.baseUrl = baseUrl;
    }

    ngOnInit() {
        this.titleService.setTitle('Group Categories');

        this.gcm.find(new GroupCategory()).subscribe(result => { this.groupcategories = result });
    }

}

export class GroupCategory {
    id: number;
    name: string;
    groups: number;
}