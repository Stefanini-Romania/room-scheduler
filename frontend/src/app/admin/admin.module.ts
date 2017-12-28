import {NgModule} from '@angular/core';
import {NgxPaginationModule} from 'ngx-pagination';
import {RouterModule, Routes} from '@angular/router';

import {AdminUsersTab} from './admin-users-tab/admin-users-tab.component';
import {AdminRoomsTab} from './admin-rooms-tab/admin-rooms-tab.component';
import {AdminComponent} from './default/admin.component';
import {CoreModule} from '../core/core.module';

const routes: Routes = [
    {path: 'admin', component: AdminComponent, data:{name: 'admin'}}
];

@NgModule({
    imports: [CoreModule,
              NgxPaginationModule,
              RouterModule.forRoot(routes)
    ],
    providers: [],
    declarations: [AdminUsersTab, AdminRoomsTab, AdminComponent],
    exports: [AdminUsersTab, AdminRoomsTab, AdminComponent],
    entryComponents: []
})
export class AdminModule {
}