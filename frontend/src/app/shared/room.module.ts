import { RoomService } from './../rooms/room.service';
import { NgModule } from '@angular/core';
// import { RSCalendarComponent } from './../calendars/default/rs-calendar.component';
import { CoreModule } from './../core/core.module';
// import { RoomSelector } from './../rooms/roomSelector.component';


@NgModule({
  imports: [ CoreModule ],
  providers: [RoomService],
  declarations: [ ],
  exports: [ ],
})
export class RoomModule {}