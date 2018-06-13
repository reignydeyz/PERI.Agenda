import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import * as $ from "jquery";

import { AccountModule, Role } from "../account/account.component";
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    private am: AccountModule

    public role: Role;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        this.am = new AccountModule();
        this.am.http = http;
        this.am.baseUrl = baseUrl;

        this.am.getRole().subscribe(r => {
            this.role = r;
        });
    }

    ngAfterViewChecked() {
        $(".navbar-collapse").collapse('hide');
    }

    /*ngAfterViewChecked() {

        // Collapse navbar when page redirects (mobile)
        // https://github.com/twbs/bootstrap/issues/12852
        $(".navbar-nav li a").click(function (event) {
            $(".navbar-collapse").collapse('hide');
        });
    }*/

    allowedRoles(roles: string): boolean {
        return roles.indexOf(this.role.name) >= 0;
    }
}