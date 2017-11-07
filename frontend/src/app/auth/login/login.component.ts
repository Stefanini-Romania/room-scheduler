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
    currentUser: User;
  
    constructor(private authService: AuthService, private router: Router,) {}
    
    ngAfterViewInit(): void {
      if(this.authService.isLoggedIn()) this.router.navigate(['/calendar']);
    }

    login() {
      this.authService.authenticate(this.model.name, this.model.password)
        .subscribe(
          data => {
              this.router.navigate(['/calendar']);
              
          },
          error => {
              this.errorMessage = "Incorrect username or password";
          })
    }

    welcome() {
      if(this.currentUser && this.currentUser.name !=null )
      this.currentUser = this.authService.getLoggedUser();
    }
  }