import {Component} from '@angular/core';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {Router} from '@angular/router';
import {TranslateService} from '@ngx-translate/core';

@Component({
    selector: 'rs-header',
    templateUrl: './rs-header.component.html'
})

export class RSHeader {
    currentUser: User = undefined;

    // @TODO download icons from https://www.iconfinder.com/iconsets/flags_gosquared
    languages = [
        {'name': 'English', 'code': 'en', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/United-Kingdom_flat.png' },
        {'name': 'Română', 'code': 'ro', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/Romania_flat.png'}
    ];

    constructor(private authService: AuthService, private translate: TranslateService, private router: Router) {
    }

    get currentLanguage() {
        return this.languages.find(l => l.code === this.translate.currentLang);
    }

    changeLanguage(lang) {
        this.translate.use(lang);
        this.translate.getTranslation(lang);
    }

    isLoggedIn(): boolean {
        this.currentUser= new User();this.currentUser.name="Costel";
        if (1) return true;
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }
}