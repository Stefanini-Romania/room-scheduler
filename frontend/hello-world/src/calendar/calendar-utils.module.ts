import { LEAVE_CLASSNAME } from '@angular/animations/browser/src/util';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  NgbDatepickerModule,
  NgbTimepickerModule
} from '@ng-bootstrap/ng-bootstrap';
import { CalendarModule } from 'angular-calendar';
import { RSCalendarHeaderComponent } from './rs-calendar-header.component';
import { DateTimePickerComponent } from './date-time-picker.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgbDatepickerModule.forRoot(),
    NgbTimepickerModule.forRoot(),
    CalendarModule
  ],
  declarations: [RSCalendarHeaderComponent, DateTimePickerComponent],
  exports: [RSCalendarHeaderComponent, DateTimePickerComponent]
})
export class DemoUtilsModule {}


