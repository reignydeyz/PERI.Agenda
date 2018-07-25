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
import { GroupEditComponent } from './components/group/group.edit.component';
import { GroupMyComponent } from './components/group/group.my.component';
import { EventNewComponent } from './components/event/event.new.component';
import { EventEditComponent } from './components/event/event.edit.component';
import { EventMyComponent } from './components/event/event.my.component';
import { ProfileComponent } from './components/profile/profile.component';

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
        EventNewComponent,
        EventEditComponent,
        EventMyComponent,
        EventCategoryComponent,
        LocationComponent,
        DashboardComponent,
        AttendanceComponent,
        GroupComponent,
        GroupCategoryComponent,
        GroupMemberComponent,
        GroupNewComponent,
        GroupEditComponent,
        GroupMyComponent,
        CalendarComponent,
        ProfileComponent
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
            { path: 'eventnew', component: EventNewComponent },
            { path: 'eventedit', component: EventEditComponent },
            { path: 'eventmy', component: EventMyComponent },
            { path: 'eventcategory', component: EventCategoryComponent },
            { path: 'location', component: LocationComponent },
            { path: 'dashboard', component: DashboardComponent },
            { path: 'attendance/:id', component: AttendanceComponent },
            { path: 'group', component: GroupComponent },
            { path: 'groupcategory', component: GroupCategoryComponent },
            { path: 'groupmember', component: GroupMemberComponent },
            { path: 'groupnew', component: GroupNewComponent },
            { path: 'groupedit', component: GroupEditComponent },
            { path: 'groupmy', component: GroupMyComponent },
            { path: 'calendar', component: CalendarComponent },
            { path: 'profile', component: ProfileComponent },

            { path: '**', redirectTo: 'dashboard' }
        ])
    ]
})
export class AppModuleShared {
}
