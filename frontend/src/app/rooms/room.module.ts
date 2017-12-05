import {RoomService} from './shared/room.service';
import {NgModule} from '@angular/core';
import {CoreModule} from './../core/core.module';
import {RoomSelector} from './room-selector/room-selector.component';
import {RoomEditorComponent} from './../rooms/room-editor/room-editor.component';

@NgModule({
    imports: [CoreModule],
    providers: [RoomService],
    declarations: [RoomSelector, RoomEditorComponent],
    exports: [RoomSelector, RoomEditorComponent],
    entryComponents:[RoomEditorComponent]
})

export class RoomModule {
}