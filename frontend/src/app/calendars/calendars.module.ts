import {NgModule} from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {RSCalendarComponent} from './default/rs-calendar.component';
import {RouterModule, Routes} from '@angular/router';
import {jqxSchedulerComponent} from './default/temp-hack/angular_jqxscheduler';
//import {jqxSchedulerComponent} from '../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {UsersModule} from '../users/users.module';
import {RoomModule} from './../rooms/room.module';
import {jqxButtonComponent} from './../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxbuttons';
import {CoreModule} from '../core/core.module';
import {SharedModule} from '../shared/shared.module';
import {EventEditorComponent} from './event-editor/event-editor.component';

const routes: Routes = [
    {path: 'calendar', component: RSCalendarComponent, data:{name: 'calendar'}}
];

@NgModule({
    imports: [
        CoreModule,
        SharedModule,
        BrowserAnimationsModule,
        RouterModule.forRoot(routes),
        RoomModule,
        UsersModule
    ],
    providers: [],
    declarations: [jqxSchedulerComponent, jqxButtonComponent, EventEditorComponent, RSCalendarComponent],
    exports: [RSCalendarComponent],
    entryComponents: [EventEditorComponent]
})

export class CalendarsModule {
}