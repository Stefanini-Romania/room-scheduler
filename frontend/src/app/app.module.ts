import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { CalendarsModule } from './calendars/calendars.module';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './auth/auth.module';

const routes: Routes = [
  // default route
  { path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  //{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    CoreModule,
    SharedModule,
    AuthModule,
    CalendarsModule,
    RouterModule.forRoot(routes),
  ],
  
  providers: [],
  bootstrap: [AppComponent]
})

export class AppModule { }
