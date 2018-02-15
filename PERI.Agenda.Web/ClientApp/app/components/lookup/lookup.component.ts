import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/toPromise';

export class LookUpComponent {
    public http: Http;
    public baseUrl: string;

    public lookups: LookUp[];

    // https://stackoverflow.com/questions/36947748/angular-2-beta-17-property-map-does-not-exist-on-type-observableresponse
    // https://stackoverflow.com/questions/37073705/property-catch-does-not-exist-on-type-observableany
    // https://stackoverflow.com/questions/38090989/property-topromise-does-not-exist-on-type-observableresponse
    public getByGroup(group: string): Promise<any> {
        /* this.http.get(this.baseUrl + 'api/lookup/findbygroup?group=' + group)
            .subscribe(result => { return result.json() as LookUp[] }, error => { return Observable.throw(error.json()) }); */

        return this.http.get(this.baseUrl + 'api/lookup/findbygroup?group=' + group).map(response => {
            console.log(response.json());
            return response.json() as LookUp[] || { success: false, message: "No response from server" };
        }).catch((error: Response | any) => {
            return Observable.throw(error.json());
        }).toPromise();
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