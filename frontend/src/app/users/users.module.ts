import {AdminComponent} from './admin/admin.component';
import {NgModule} from '@angular/core';
import {AuthModule} from '../auth/auth.module';
import {RouterModule, Routes} from '@angular/router';
import {CoreModule} from './../core/core.module';
import {UserService} from '../users/shared/users.service';
import {LoginPageComponent} from './user-login/login-page.component';
import {LoginFormComponent} from './user-login/login-form.component';
import {RegisterFormComponent} from './user-register/register-form.component';
import {RegisterPageComponent} from './user-register/register-page.component';

const routes: Routes = [
    {path: 'login', component: LoginPageComponent},
    {path: 'register', component: RegisterPageComponent},
    {path: 'admin', component: AdminComponent}
];

@NgModule({
    imports: [
        AuthModule,
        RouterModule.forRoot(routes),
        CoreModule,
    ],
    providers: [UserService],
    declarations: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, AdminComponent],
    exports: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, AdminComponent],
    entryComponents: [LoginFormComponent]
})

export class UsersModule {

}