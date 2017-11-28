import { AdminComponent } from './admin/admin.component';
import {NgModule} from '@angular/core';
import {AuthModule} from '../auth/auth.module';
import {RouterModule, Routes} from '@angular/router';
import {CoreModule} from './../core/core.module';
import {UserService} from '../users/shared/users.service';
import { User } from '../shared/models/user.model';

const routes: Routes = [
    {path: 'admin', component: AdminComponent}
];

@NgModule({
    imports: [
        AuthModule,
        RouterModule.forRoot(routes),
        CoreModule,
    ],
    providers: [UserService],
    declarations: [AdminComponent],
    exports: [AdminComponent],
})

export class UsersModule {

}