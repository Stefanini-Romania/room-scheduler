import { RSCalendarComponent } from '../calendar/rs-calendar.component';

import { RSCalendarModule } from '../calendar/rs-calendar.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule }   from '@angular/forms';
import { AppComponent } from './app.component';
import { RSNavbar } from './rs-navbar.component';
import { RSFooter } from './rs-footer.component';
import {LoginComponent} from './login.component';


// import {FlexLayoutModule} from "@angular/flex-layout";
// import { routing, appRoutingProviders }  from './app.routing';
// import { AppServices } from './app.services';
// import {AuthGuard} from "./auth/auth.guard";
// import {AppComponent} from './app.component';
// import {AuthComponent} from './auth/auth.component';
// import {NotFoundComponent} from './404/not-found.component';
// import { HomeComponent } from './home/home.component';


const appRoutes: Routes = [

  {
    path: 'login',
    component: LoginComponent
  },
  // default route
  { path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },



 
  
  //{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent, RSNavbar, RSFooter, LoginComponent, 
  ],
  imports: [
    BrowserModule,
    RSCalendarModule,
    FormsModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    ),
  ],
  
  providers: [],
  bootstrap: [AppComponent, ]
})
export class AppModule { }
