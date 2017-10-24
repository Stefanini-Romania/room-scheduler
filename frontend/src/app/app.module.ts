import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { FormsModule } from '@angular/forms';
import { CalendarsModule } from './calendars/calendars.module';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './auth/auth.module';
import { AuthService } from './auth/shared/auth.service';
//import { HttpModule, JsonpModule } from '@angular/http';

const routes: Routes = [
  // default route
  { path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  //{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    //HttpModule,
    CoreModule,
    SharedModule,
    AuthModule,
    //JsonpModule,
    CalendarsModule,
    RouterModule.forRoot(routes),
  ],
  
  providers: [AuthService],
  declarations: [AppComponent],
  exports: [],
  bootstrap: [AppComponent]
  })

export class AppModule { }
