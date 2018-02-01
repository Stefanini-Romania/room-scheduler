import {NgModule} from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {CommonModule} from "@angular/common";
import {TranslateModule} from '@ngx-translate/core';
import {LocalizedDatePipe} from './pipes/localized-date.pipe';
import {KeysPipe} from './pipes/keys.pipe';
import {ValidateEqualValidator} from './validators/validate-equal-validator.directive';
import {HostAvailability} from './hosts/host-availability.component';
import {HostAvailabilityForm} from './hosts/host-availability-form/host-availability-form.component';
import { FormsModule } from '@angular/forms';
import {RoomModule} from '../rooms/room.module';


@NgModule({
    imports: [CommonModule,
                TranslateModule,
                NgbModule.forRoot(),
                FormsModule,
                RoomModule],
    providers: [],
    declarations: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator, HostAvailability, HostAvailabilityForm],
    exports: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator, HostAvailability, HostAvailabilityForm],
    entryComponents: [HostAvailabilityForm]
})

export class SharedModule {
}
