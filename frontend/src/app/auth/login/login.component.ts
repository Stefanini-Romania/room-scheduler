import { Component } from '@angular/core';
import { User } from '../../shared/user.model';
import { AuthService } from '../shared/auth.service';

@Component({
    selector: 'login-component',
    templateUrl: './login.component.html',
    styleUrls: [],
    providers: [AuthService]
  })

  export class LoginComponent{
    model: User = <User>{};
    activeUser: User;
  
    constructor(private authService: AuthService) {}
  
    authenticate() {
      this.authService.authenticate(this.model.UserName, this.model.Password);
    }
  
    selectUser(user) {
      this.activeUser = user;
      console.log(this.activeUser);
    }
   
}