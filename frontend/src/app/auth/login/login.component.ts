import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../shared/user.model';
import { AuthService } from '../shared/auth.service';
import { AuthModule } from '../auth.module';
import { Observable } from 'rxjs/Observable';

@Component ({
    selector: 'login-component',
    templateUrl: './login.component.html',
    styleUrls: [],
    providers: [AuthService],
  })

  export class LoginComponent {
    public errorMessage: string = '';

    model: User = <User> {};
    activeUser: User;
  
    constructor(private authService: AuthService, private router: Router,) {}
    
    login() {
      this.authService.authenticate(this.model.UserName, this.model.Password)
        .subscribe(
          data => {
              this.router.navigate(['/calendar']);
              
          },
          error => {
              this.errorMessage = "Incorrect username or password";
          })
    }
  }