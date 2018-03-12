import { Component, AfterViewInit } from '@angular/core';
import * as $ from "jquery";

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {

    ngAfterViewInit() {

        // Collapse navbar when page redirects (mobile)
        // https://github.com/twbs/bootstrap/issues/12852
        $(".navbar-nav li a").click(function (event) {
            $(".navbar-collapse").collapse('hide');
        });
    }

}