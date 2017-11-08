import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RSCalendarComponent } from './default/rs-calendar.component';
import { RouterModule, Routes } from '@angular/router';
import { jqxSchedulerComponent } from '../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import { RoomSelector } from './../rooms/roomSelector.component';
import { RoomModule } from './../rooms/room.module';
import { FormsModule } from '@angular/forms';


const routes : Routes =[
  {path: 'calendar', component:  RSCalendarComponent }
];

@NgModule({
  imports: [
    FormsModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes),
    RoomModule
  ],
  providers: [],
  declarations: [jqxSchedulerComponent, RSCalendarComponent, RoomSelector],
  exports: [RSCalendarComponent],
})

export class CalendarsModule {}