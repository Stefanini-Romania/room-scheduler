import {Component} from '@angular/core';

import {RoleEnum} from '../../shared/models/role.model';
import {DepartmentIdEnum} from '../../shared/models/departmentIdEnum.model';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {UserService} from '../shared/users.service';

@Component({
    moduleId: module.id,
    selector: 'register-form',
    templateUrl: './register-form.component.html',
    styleUrls: [],
    providers: [AuthService],
})

export class RegisterFormComponent {
    public confirmPassword;

    public model: User = <User>{
        departmentId: DepartmentIdEnum.ADC,
        userRoles: [RoleEnum.attendee]
    };
    
    constructor(private userService: UserService) {
    }

    register() {
        this.userService.create(this.model);
    }
}