import {NgModule} from '@angular/core';
import {CommonModule} from "@angular/common";
import {LocalizedDatePipe} from './pipes/localized-date.pipe';
import {KeysPipe} from './pipes/keys.pipe';
import {ValidateEqualValidator} from './validators/validate-equal-validator.directive';
import {HostAvailability} from './hosts/host-availability.component';

@NgModule({
    imports: [CommonModule],
    providers: [],
    declarations: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator, HostAvailability],
    exports: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator, HostAvailability]
})

export class SharedModule {
}
