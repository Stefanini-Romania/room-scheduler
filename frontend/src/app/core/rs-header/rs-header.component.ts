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

    languages = [
        {'name': 'English', 'code': 'en'},
        {'name': 'Română', 'code': 'ro'}
    ];

    constructor(private authService: AuthService, private translate: TranslateService, private router: Router) {
    }

    get currentLanguage() {
        return this.languages.find(l => l.code === this.translate.currentLang);
    }

    changeLanguage(lang) {
        console.log(lang);
        this.translate.use(lang);
        this.translate.getTranslation(lang);
    }

    isLoggedIn(): boolean {
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }
}