import { Component } from '@angular/core';
import { User } from './models/user';
import { AuthService } from './services/auth.service';

@Component({
    selector: 'login-component',
    templateUrl: './login.component.html',
    styleUrls: ['./app.component.css'],
    providers: [AuthService]
  })

  export class LoginComponent{
    model: User = <User>{};
    activeUser: User;
  
    //constructor(private authService: AuthService) {}
  
    authenticate() {
      alert('ok');
      //this.authService.authenticate(this.model.UserName, this.model.Password);
    }
  
    selectUser(user) {
      this.activeUser = user;
      console.log(this.activeUser);
    }
   
}