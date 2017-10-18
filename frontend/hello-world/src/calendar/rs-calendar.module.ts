import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule } from 'angular-calendar';
import { DemoUtilsModule } from './calendar-utils.module';
import { RSCalendarComponent } from './rs-calendar.component';
import { RouterModule, Routes } from '@angular/router';

const CalendarRoutes : Routes =[
  {path: 'calendar',  component: RSCalendarComponent}
]

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    CalendarModule.forRoot(),
    DemoUtilsModule,
    RouterModule.forRoot(
      CalendarRoutes, {enableTracing: true})
  ],
  declarations: [RSCalendarComponent],
  exports: [RSCalendarComponent],
  //bootstrap: [RSCalendarComponent]
})
export class RSCalendarModule {}