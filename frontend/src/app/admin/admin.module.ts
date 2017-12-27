import {NgModule} from '@angular/core';
import {NgxPaginationModule} from 'ngx-pagination';

import {AdminUsersTab} from './admin-users-tab/admin-users-tab.component';
import {CoreModule} from '../core/core.module';

@NgModule({
    imports: [CoreModule,
            NgxPaginationModule
    ],
    providers: [],
    declarations: [AdminUsersTab],
    exports: [AdminUsersTab],
    entryComponents: []
})
export class AdminModule {
}