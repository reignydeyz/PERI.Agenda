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
import { ReportTemplateComponent } from './components/reporttemplate/reporttemplate.component';
import { ActivityReportComponent } from './components/activityreport/activityreport.component';
import { DashboardService } from './services/dashboard.service';
import { AccountService } from './services/account.service';
import { MemberService } from './services/member.service';
import { GroupService } from './services/group.service';
import { RoleService } from './services/role.service';
import { GroupCategoryService } from './services/groupcategory.service';
import { ErrorExceptionModule } from './components/errorexception/errorexception.component';
import { ActivityReportService } from './services/activityreport.service';
import { ReportService } from './services/report.service';
import { EventCategoryService } from './services/eventcategory.service';
import { GroupMemberService } from './services/groupmember.service';
import { LocationService } from './services/location.service';
import { EventService } from './services/event.service';
import { AttendanceService } from './services/attendance.service';
import { AuthGuardService as AuthGuard } from './services/auth-guard.service';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ResponseInterceptor } from './interceptors/res.interceptor';

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
        ProfileComponent,
        ReportTemplateComponent,
        ActivityReportComponent
    ],
    imports: [
        HttpClientModule,
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
            { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]  },
            { path: 'attendance/:id', component: AttendanceComponent },
            { path: 'group', component: GroupComponent },
            { path: 'groupcategory', component: GroupCategoryComponent },
            { path: 'groupmember', component: GroupMemberComponent },
            { path: 'groupnew', component: GroupNewComponent },
            { path: 'groupedit', component: GroupEditComponent },
            { path: 'groupmy', component: GroupMyComponent },
            { path: 'calendar', component: CalendarComponent },
            { path: 'profile', component: ProfileComponent },
            { path: 'reporttemplate', component: ReportTemplateComponent },
            { path: 'activityreport', component: ActivityReportComponent },

            { path: '**', redirectTo: 'dashboard' }
        ])
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: ResponseInterceptor, multi: true },
        AccountService,
        DashboardService,
        MemberService,
        GroupService,
        GroupCategoryService,
        RoleService,
        ErrorExceptionModule,
        ActivityReportService,
        ReportService,
        EventCategoryService,
        GroupMemberService,
        LocationService,
        EventService,
        AttendanceService,
        AuthGuard
    ],
})
export class AppModuleShared {
}
