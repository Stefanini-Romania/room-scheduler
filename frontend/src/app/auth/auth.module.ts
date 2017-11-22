import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginFormComponent} from './login/login-form.component';
import { LoginComponent} from './login/login-page.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AuthService } from './shared/auth.service';

const routes: Routes = [
    { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [
      CoreModule,
      SharedModule,
      RouterModule.forRoot(routes),
  ],
  providers: [AuthService],
  declarations: [LoginFormComponent, LoginComponent],
  exports: [LoginFormComponent, LoginComponent],
})
export class AuthModule {}