import {Component} from '@angular/core';

import {RoleIdEnum} from '../../shared/models/roleIdEnum.model';
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
    public model: User = <User>{
        firstName: '',
        lastName: '',
        name: '',
        email: '',
        password: '',  
        confirmPassword: '', 
        departmentId: DepartmentIdEnum.ADC,
        roleId: RoleIdEnum.attendee
    };
    
    constructor(private userService: UserService) {
    }

    register() {
        this.userService.create(this.model);
    }
}