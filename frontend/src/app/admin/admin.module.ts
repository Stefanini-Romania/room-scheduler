import {NgModule} from '@angular/core';
import {NgxPaginationModule} from 'ngx-pagination';
import {RouterModule, Routes} from '@angular/router';

import {AdminUsersTab} from './admin-users-tab/admin-users-tab.component';
import {AdminRoomsTab} from './admin-rooms-tab/admin-rooms-tab.component';
import {AdminComponent} from './default/admin.component';
import {AdminSystemParameters} from './admin-system-parameters/admin-system-parameters.component';
import {SystemParametersService} from './shared/system-parameters.service';
import {CoreModule} from '../core/core.module';

const routes: Routes = [
    {path: 'admin', component: AdminComponent, data:{name: 'admin'}}
];

@NgModule({
    imports: [CoreModule,
              NgxPaginationModule,
              RouterModule.forRoot(routes)
    ],
    providers: [SystemParametersService],
    declarations: [AdminUsersTab, AdminRoomsTab, AdminComponent, AdminSystemParameters],
    exports: [AdminUsersTab, AdminRoomsTab, AdminComponent, AdminSystemParameters],
    entryComponents: []
})
export class AdminModule {
}