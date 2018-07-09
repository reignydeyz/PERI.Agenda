import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';

import { AccountComponent } from './components/account/account.component';
import { MemberComponent } from './components/member/member.component';
import { EventComponent } from './components/event/event.component';
import { EventCategoryComponent } from './components/eventcategory/eventcategory.component';
import { LocationComponent } from './components/location/location.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AttendanceComponent } from './components/attendance/attendance.component';
import { GroupComponent } from './components/group/group.component';
import { GroupCategoryComponent } from './components/groupcategory/groupcategory.component';
import { GroupMemberComponent } from './components/groupmember/groupmember.component';
import { CalendarComponent } from './components/calendar/calendar.component';

import { ChartsModule } from 'ng2-charts';
import { MyDatePickerModule } from 'mydatepicker';
import { GroupNewComponent } from './components/group/group.new.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,

        AccountComponent,
        MemberComponent,
        EventComponent,
        EventCategoryComponent,
        LocationComponent,
        DashboardComponent,
        AttendanceComponent,
        GroupComponent,
        GroupCategoryComponent,
        GroupMemberComponent,
        GroupNewComponent,
        CalendarComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ChartsModule,
        MyDatePickerModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },

            { path: 'account', component: AccountComponent },
            { path: 'member', component: MemberComponent },
            { path: 'event', component: EventComponent },
            { path: 'eventcategory', component: EventCategoryComponent },
            { path: 'location', component: LocationComponent },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'attendance/:id', component: AttendanceComponent },
            { path: 'group', component: GroupComponent },
            { path: 'groupcategory', component: GroupCategoryComponent },
            { path: 'groupmember', component: GroupMemberComponent },
            { path: 'groupnew', component: GroupNewComponent },
            { path: 'calendar', component: CalendarComponent },

            { path: '**', redirectTo: 'dashboard' }
        ])
    ]
})
export class AppModuleShared {
}
