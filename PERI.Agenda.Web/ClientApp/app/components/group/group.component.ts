import { Component } from '@angular/core';
import { Http } from '@angular/http';

export class GroupModule {
    public http: Http;
    public baseUrl: string;
}

@Component({
    selector: 'group',
    templateUrl: './group.component.html'
})
export class GroupComponent {
}

export class Group {
    id: number;
    groupCategoryId: number;
    category: string;
    name: string;
}