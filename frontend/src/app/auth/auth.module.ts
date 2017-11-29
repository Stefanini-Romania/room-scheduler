import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginFormComponent} from '../users/user-login/login-form.component';
import {LoginPageComponent} from '../users/user-login/login-page.component';
import {CoreModule} from '../core/core.module';
import {SharedModule} from '../shared/shared.module';
import {AuthService} from './shared/auth.service';
import {UsersModule} from '../users/users.module';



const routes: Routes = [
   // { path: 'login', component: LoginPageComponent }
];

  imports: [
      CoreModule,
      SharedModule,
      UsersModule,
      RouterModule.forRoot(routes),
  ],
  providers: [AuthService],
})
export class AuthModule {}