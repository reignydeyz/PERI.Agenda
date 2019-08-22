import { Component, Inject, AfterViewInit } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import * as $ from "jquery";

import { Observable } from 'rxjs/Observable';
import { AccountService } from '../../services/account.service';
import { Role } from '../../models/role';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    public role: Role;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string,
    private am: AccountService) {
    }

    async ngOnInit() {
        this.role = await this.am.getRole();
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