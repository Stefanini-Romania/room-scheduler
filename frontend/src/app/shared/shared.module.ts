import {NgModule} from '@angular/core';
import {LocalizedDatePipe} from './pipes/localized-date.pipe';

@NgModule({
    imports: [],
    providers: [],
    declarations: [LocalizedDatePipe],
    exports: [LocalizedDatePipe],
})
export class SharedModule {
}