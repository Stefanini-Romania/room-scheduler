import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule, CalendarEventTitleFormatter } from 'angular-calendar';
import { CustomEventTitleFormatter } from './custom-event-title-formatter.provider';
import { RSCalendarComponent } from './default/rs-calendar.component';
import { RouterModule, Routes } from '@angular/router';
import {RSCalendarHeaderComponent} from './default/rs-calendar-header.component';
import {DateTimePickerComponent} from './shared/date-time-picker.component';
import {
    NgbDatepickerModule,
    NgbTimepickerModule
} from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

const routes : Routes =[
  {path: 'calendar', component:  RSCalendarComponent }
];


@NgModule({
  imports: [
    FormsModule,
    BrowserAnimationsModule,
    CalendarModule.forRoot(),
    NgbDatepickerModule.forRoot(),
    NgbTimepickerModule.forRoot(),
    RouterModule.forRoot(routes)
  ],
  providers: [
    { provide: CalendarEventTitleFormatter, useClass: CustomEventTitleFormatter}
  ],
  declarations: [RSCalendarComponent, RSCalendarHeaderComponent, DateTimePickerComponent],
  exports: [RSCalendarComponent],
})
export class CalendarsModule {}