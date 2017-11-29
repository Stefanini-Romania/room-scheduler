import {AdminComponent} from './admin/admin.component';
import {Register} from 'ts-node/dist';
import {RegisterFormComponent} from './user-register/register-form.component';
import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {User} from '../shared/models/user.model';
import {SharedModule} from '../shared/shared.module';
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
        
        RouterModule.forRoot(routes),
        CoreModule,
        SharedModule
    ],
    providers: [UserService],
    declarations: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, AdminComponent],
    exports: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, AdminComponent],
    entryComponents: [LoginFormComponent]
})

export class UsersModule {

}