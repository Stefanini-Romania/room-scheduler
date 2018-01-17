import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {NgModule} from '@angular/core';
import {TranslateModule} from '@ngx-translate/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import {RsHeaderComponent} from './rs-header/rs-header.component';
import {RsFooterComponent} from './rs-footer/rs-footer.component';
import {SharedModule} from '../shared/shared.module';
import {LanguageSelector} from './language-selector/language-selector';
import {DialogService} from '../shared/services/dialog.service';
import {RSDialogContentComponent} from './rs-dialog-content/rs-dialog-content.component';
import {APIRequestInterceptor} from './../auth/shared/api-request-interceptor';

@NgModule({
    imports: [CommonModule, FormsModule, HttpModule, NgbModule, TranslateModule, SharedModule],
    providers: [DialogService, APIRequestInterceptor],
    declarations: [RsHeaderComponent, RsFooterComponent, LanguageSelector, RSDialogContentComponent],
    exports: [CommonModule, FormsModule, TranslateModule, NgbModule, RsHeaderComponent, RsFooterComponent],
    entryComponents: [RSDialogContentComponent]
})
export class CoreModule {
}