import {NgModule} from '@angular/core';
import {LocalizedDatePipe} from './pipes/localized-date.pipe';
import {ValidateEqualValidator} from './validators/validate-equal-validator.directive';

@NgModule({
    imports: [],
    providers: [],
    declarations: [LocalizedDatePipe, ValidateEqualValidator],
    exports: [LocalizedDatePipe, ValidateEqualValidator],
})
export class SharedModule {
}
