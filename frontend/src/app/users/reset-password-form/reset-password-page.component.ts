import {Component} from '@angular/core';
import {Router} from '@angular/router';

@Component({
    selector: 'reset-password-component',
    template: `
        <reset-password-form (emailSent)="onEmailSent()"></reset-password-form>
        <rs-footer></rs-footer>
    `,
    styleUrls: [],
    providers: []

})

export class ResetPasswordPageComponent {

    constructor(private router: Router) {}

    onEmailSent() {
        return this.router.navigate(['/login']);
    }

}