import {RoomService} from './shared/room.service';
import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { BrowserModule } from '@angular/platform-browser';


import {TranslateModule} from '@ngx-translate/core';
// import {CoreModule} from './../core/core.module';
import {RoomSelector} from './room-selector/room-selector.component';
import {RoomEditorComponent} from './../rooms/room-editor/room-editor.component';

@NgModule({
    imports: [FormsModule,
            HttpModule,
            NgbModule.forRoot(),
            TranslateModule,
            BrowserModule
    ],
    providers: [RoomService],
    declarations: [RoomSelector, RoomEditorComponent],
    exports: [RoomSelector, RoomEditorComponent],
    entryComponents:[RoomEditorComponent]
})

export class RoomModule {
}