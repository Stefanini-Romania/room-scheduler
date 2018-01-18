import {Register} from 'ts-node/dist';
import {NgxPaginationModule} from 'ngx-pagination';
import {AdminModule} from '../admin/admin.module';

import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CoreModule} from '../core/core.module';
import {SharedModule} from '../shared/shared.module';
import {UserService} from './shared/users.service';
import {LoginPageComponent} from './login-form/login-page.component';
import {LoginFormComponent} from './login-form/login-form.component';
import {RegisterFormComponent} from './register-form/register-form.component';
import {RegisterPageComponent} from './register-form/register-page.component';
import {ResetPasswordFormComponent} from './reset-password-form/reset-password-form.component';
import {ResetPasswordPageComponent} from './reset-password-form/reset-password-page.component';


const routes: Routes = [
    {path: 'login', component: LoginPageComponent, data:{name: 'login'}},
    {path: 'register', component: RegisterPageComponent, data:{name: 'register'}},
    {path: 'resetpass', component: ResetPasswordPageComponent, data:{name: 'resetpass'}}
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes),
        CoreModule,
        SharedModule,
        NgxPaginationModule,
        AdminModule
    ],
    providers: [UserService],
    declarations: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, ResetPasswordFormComponent, ResetPasswordPageComponent],
    exports: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, ResetPasswordFormComponent, ResetPasswordPageComponent],
    entryComponents: [LoginFormComponent, RegisterFormComponent, ResetPasswordFormComponent]
})

export class UsersModule {

}