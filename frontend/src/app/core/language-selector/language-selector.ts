import {Component, Input} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {NgbDropdownConfig} from '@ng-bootstrap/ng-bootstrap';

export interface ILanguage {
    name: string;
    code: string;
    icon?: string;
}

@Component({
    selector: 'language-selector',
    templateUrl: './language-selector.html',
    providers: [NgbDropdownConfig]
})

export class LanguageSelector {
    // @TODO validate if languages are empty or not
    @Input()
    languages: ILanguage[];

    constructor(config: NgbDropdownConfig, private translate: TranslateService) {
        config.placement = 'bottom-right';
    }

    get currentLanguage() {
        return this.languages.find(l => l.code === this.translate.currentLang);
    }

    changeLanguage(lang) {
        this.translate.use(lang);
        sessionStorage.setItem('language', lang);
    }
}
