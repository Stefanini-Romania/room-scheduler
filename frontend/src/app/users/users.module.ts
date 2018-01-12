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
import {LostPasswordFormComponent} from './lost-password-form/lost-password-form.component';
import {LostPasswordPageComponent} from './lost-password-form/lost-password-page.component';


const routes: Routes = [
    {path: 'login', component: LoginPageComponent, data:{name: 'login'}},
    {path: 'register', component: RegisterPageComponent, data:{name: 'register'}},
    {path: 'lostpassword', component: LostPasswordPageComponent, data:{name: 'lostpassword'}}
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
    declarations: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, LostPasswordFormComponent, LostPasswordPageComponent],
    exports: [LoginFormComponent, LoginPageComponent, RegisterPageComponent, RegisterFormComponent, LostPasswordFormComponent, LostPasswordPageComponent],
    entryComponents: [LoginFormComponent, RegisterFormComponent, LostPasswordFormComponent]
})

export class UsersModule {

}