import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';

import { MemberComponent } from './components/member/member.component';
import { EventComponent } from './components/event/event.component';
import { EventCategoryComponent } from './components/eventcategory/eventcategory.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,

        MemberComponent,
        EventComponent,
        EventCategoryComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },

            { path: 'member', component: MemberComponent },
            { path: 'event', component: EventComponent },
            { path: 'eventcategory', component: EventCategoryComponent },

            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
