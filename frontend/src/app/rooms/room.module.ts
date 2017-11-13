import {RoomService} from './shared/room.service';
import {NgModule} from '@angular/core';
import {CoreModule} from './../core/core.module';
import {RoomSelector} from './room-selector/room-selector.component';

@NgModule({
    imports: [CoreModule],
    providers: [RoomService],
    declarations: [RoomSelector],
    exports: [RoomSelector]
})

export class RoomModule {
}