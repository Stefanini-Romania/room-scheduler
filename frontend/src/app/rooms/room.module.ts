import { RoomService } from './../rooms/room.service';
import { NgModule } from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from './../core/core.module';
// import { RSCalendarComponent } from './../calendars/default/rs-calendar.component';
// import { RoomSelector } from './../rooms/roomSelector.component';


@NgModule({
  imports: [ CoreModule ],
  providers: [RoomService],
  declarations: [ ],
  exports: [NgbModule]
})

export class RoomModule {}