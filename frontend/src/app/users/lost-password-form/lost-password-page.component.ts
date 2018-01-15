import {Component} from '@angular/core';
import {Router} from '@angular/router';

@Component({
    selector: 'lost-password-component',
    template: `
        <lost-password-form></lost-password-form>
        <rs-footer></rs-footer>
    `,
    styleUrls: [],
    providers: []

})

export class LostPasswordPageComponent {

    constructor(private router: Router) {}

}