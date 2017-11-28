import {Register} from 'ts-node/dist';
import { AdminComponent } from './admin/admin.component';
import {RegisterFormComponent} from './user-register/register-form.component';
import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CoreModule} from './../core/core.module';
import {UserService} from '../users/shared/users.service';
import {LoginPageComponent} from './user-login/login-page.component';
import {LoginFormComponent} from './user-login/login-form.component';
import {RegisterPageComponent} from './user-register/register-page.component';


const routes: Routes = [
    {path: 'admin', component: AdminComponent},
    {path: 'login', component: LoginPageComponent},
    {path: 'register', component: RegisterFormComponent}
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes),
        CoreModule
    ],
    providers: [UserService],
    declarations: [AdminComponent, LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent],
    exports: [AdminComponent, LoginFormComponent, RegisterFormComponent],
    entryComponents: [LoginFormComponent]
})

export class UsersModule {

}