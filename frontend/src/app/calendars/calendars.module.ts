import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RSCalendarComponent} from '../calendars/default/rs-calendar.component'
import { RouterModule, Routes } from '@angular/router';
import {SchedulerModule} from '../../../node_modules/jqwidgets-framework/demos/angular/app/modules/scheduler.module';

import { FormsModule } from '@angular/forms';

const routes : Routes =[
  {path: 'calendar', component:  RSCalendarComponent }
];


@NgModule({
  imports: [
    FormsModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes),
    SchedulerModule
  ],
  providers: [
    
  ],
  declarations: [RSCalendarComponent],
  exports: [RSCalendarComponent],
})
export class CalendarsModule {}