import {Router} from '@angular/router';
import {User} from '../../shared/models/user.model';
import {Component} from '@angular/core';

@Component({
    selector: 'lost-password-form',
    templateUrl: './lost-password-form.component.html',
    styleUrls: [],
    providers: [],
})

export class LostPasswordFormComponent{

    constructor(public router: Router) {
    }
}