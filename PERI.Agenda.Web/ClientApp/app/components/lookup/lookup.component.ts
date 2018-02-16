import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable';

export class LookUpComponent {
    public http: Http;
    public baseUrl: string;

    // http://blog.ninja-squad.com/2016/03/15/ninja-tips-2-type-your-json-with-typescript/
    public getByGroup(group: string) : Observable<LookUp[]> {        
        return this.http.get('api/lookup/findbygroup?group=' + group)
            .map(response => response.json());
    }
}

export class LookUp {
    id: number;
    group: string;
    name: string;
    value: string;
    description: string;
    weight: number;
}