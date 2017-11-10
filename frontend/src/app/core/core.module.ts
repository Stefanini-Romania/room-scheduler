import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {NgModule} from '@angular/core';
import {TranslateModule} from '@ngx-translate/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import {RSHeader} from './rs-header/rs-header.component';
import {RSFooter} from './rs-footer/rs-footer.component';
import {SharedModule} from '../shared/shared.module';
import {LanguageSelector} from './language-selector/language-selector';

@NgModule({
    imports: [CommonModule, FormsModule, HttpModule, NgbModule, SharedModule],
    providers: [],
    declarations: [RSHeader, RSFooter, LanguageSelector],
    exports: [CommonModule, FormsModule, TranslateModule, NgbModule, RSHeader, RSFooter],
})
export class CoreModule {
}