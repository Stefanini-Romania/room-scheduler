import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AuthService } from './shared/auth.service';
import { APIRequestInterceptor } from './shared/api-request-interceptor';

@NgModule({
  imports: [
      CoreModule,
      SharedModule,
  ],
  providers: [
    AuthService,
    { provide: HTTP_INTERCEPTORS, useClass: APIRequestInterceptor, multi: true }
],

})
export class AuthModule {}
