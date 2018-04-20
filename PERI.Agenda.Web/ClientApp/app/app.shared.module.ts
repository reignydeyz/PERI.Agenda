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
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AttendanceComponent } from './components/attendance/attendance.component';
import { GroupComponent } from './components/group/group.component';
import { GroupCategoryComponent } from './components/groupcategory/groupcategory.component';
import { Pager } from './components/pager/pager.component';

import { ChartsModule } from 'ng2-charts';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,

        MemberComponent,
        EventComponent,
        EventCategoryComponent,
        DashboardComponent,
        AttendanceComponent,
        GroupComponent,
        GroupCategoryComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ChartsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },

            { path: 'member', component: MemberComponent },
            { path: 'event', component: EventComponent },
            { path: 'eventcategory', component: EventCategoryComponent },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'attendance/:id', component: AttendanceComponent },
            { path: 'group', component: GroupComponent },
            { path: 'groupcategory', component: GroupCategoryComponent },

            { path: '**', redirectTo: 'dashboard' }
        ])
    ]
})
export class AppModuleShared {
}
