import {NgModule} from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {CommonModule} from "@angular/common";
import {TranslateModule} from '@ngx-translate/core';
import {LocalizedDatePipe} from './pipes/localized-date.pipe';
import {KeysPipe} from './pipes/keys.pipe';
import {ValidateEqualValidator} from './validators/validate-equal-validator.directive';
import {HostAvailability} from './hosts/host-availability/host-availability.component';
import {HostAvailabilityForm} from './hosts/host-availability-form/host-availability-form.component';
import {HostExceptionForm} from './hosts/host-exception-form/host-exception-form.component';
import {HostSelector} from './../shared/hosts/host-selector/host-selector.component';

import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RoomModule} from '../rooms/room.module';
import { HostService } from './services/host.service';


@NgModule({
    imports: [CommonModule,
                TranslateModule,
                NgbModule.forRoot(),
                FormsModule,
                RoomModule,
                ReactiveFormsModule],

    providers: [HostService],
    declarations: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator, HostAvailability, HostAvailabilityForm, HostExceptionForm, HostSelector],
    exports: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator, HostAvailability, HostAvailabilityForm, HostExceptionForm, HostSelector],
    entryComponents: [HostAvailabilityForm, HostExceptionForm]
})

export class SharedModule {
}
