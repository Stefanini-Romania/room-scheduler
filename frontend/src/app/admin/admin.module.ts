import {NgModule} from '@angular/core';
import {NgxPaginationModule} from 'ngx-pagination';

import {AdminUsersTab} from './admin-users-tab/admin-users-tab.component';
import {AdminRoomsTab} from './admin-rooms-tab/admin-rooms-tab.component';
import {CoreModule} from '../core/core.module';

@NgModule({
    imports: [CoreModule,
              NgxPaginationModule
    ],
    providers: [],
    declarations: [AdminUsersTab, AdminRoomsTab],
    exports: [AdminUsersTab, AdminRoomsTab],
    entryComponents: []
})
export class AdminModule {
}