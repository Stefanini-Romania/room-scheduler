import {NgModule} from '@angular/core';
import { LocalizedDatePipe } from './pipes/localized-date.pipe';
import { KeysPipe } from './pipes/keys.pipe';
import {ValidateEqualValidator} from './validators/validate-equal-validator.directive';

@NgModule({
    imports: [],
    providers: [],
    declarations: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator],
    exports: [KeysPipe, LocalizedDatePipe, ValidateEqualValidator],
})
export class SharedModule {
}
