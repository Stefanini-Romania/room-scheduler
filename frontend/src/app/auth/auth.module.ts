import { NgModule } from '@angular/core';

import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AuthService } from './shared/auth.service';

@NgModule({
  imports: [
      CoreModule,
      SharedModule,
  ],
  providers: [AuthService],
})
export class AuthModule {}
