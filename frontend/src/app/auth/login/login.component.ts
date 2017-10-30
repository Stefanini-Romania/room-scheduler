import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../shared/user.model';
import { AuthService } from '../shared/auth.service';
import { AuthModule } from '../auth.module';

@Component ({
    selector: 'login-component',
    templateUrl: './login.component.html',
    styleUrls: [],
    providers: [AuthService]
  })

  export class LoginComponent {
  
    model: User = <User>{};
    activeUser: User;
  
    constructor(private authService: AuthService, private router: Router,) {}
    
    
    authenticate() {
      this.authService.login(this.model.UserName, this.model.Password);
      this.router.navigate(['/calendar'])
      {alert("Bine ai venit, " + this.model.UserName + "!");}
    }
  
    selectUser(user) {
      this.activeUser = user;
      console.log(this.activeUser);
    }
  }