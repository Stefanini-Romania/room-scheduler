import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginFormComponent} from '../users/user-login/login-form.component';
import { LoginPageComponent} from '../users/user-login/login-page.component';
import {RegisterFormComponent} from '../users/user-register/register-form.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AuthService } from './shared/auth.service';

const routes: Routes = [
    { path: 'login', component: LoginPageComponent }
];

@NgModule({
  imports: [
      CoreModule,
      SharedModule,
      RouterModule.forRoot(routes),
  ],
  providers: [AuthService],
  declarations: [LoginFormComponent, LoginPageComponent, RegisterFormComponent],
  exports: [LoginFormComponent, LoginPageComponent, RegisterFormComponent],
  entryComponents: [LoginFormComponent]
})
export class AuthModule {}