import {Component, Input} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';

export interface ILanguage {
    name: string;
    code: string;
    icon?: string;
}

@Component({
    selector: 'language-selector',
    templateUrl: './language-selector.html'
})

export class LanguageSelector {
    // @TODO validate if languages are empty or not
    @Input()
    languages: ILanguage[];

    constructor(private translate: TranslateService) {}

    get currentLanguage() {
        return this.languages.find(l => l.code === this.translate.currentLang);
    }

    changeLanguage(lang) {
        this.translate.use(lang);
    }
}
