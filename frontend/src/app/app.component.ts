import {Component} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    providers: [],
    styleUrls: []
})

export class AppComponent {
    constructor(translate: TranslateService) {
        // this language will be used as a fallback when a translation isn't found in the current language
        translate.setDefaultLang('ro');

        // the lang to use, if the lang isn't available, it will use the current loader to get them
        translate.use('en');
    }
}

