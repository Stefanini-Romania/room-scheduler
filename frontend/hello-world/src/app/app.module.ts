import { RSCalendarComponent } from '../calendar/rs-calendar.component';

import { RSCalendarModule } from '../calendar/rs-calendar.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { RSNavbar } from './rs-navbar.component';
import {RSContent} from './rs-content.component';
import {RSFooter} from './rs-footer.component';
import {LoginComponent} from './login.component';



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
    AppComponent, RSNavbar, RSContent, RSFooter, LoginComponent
  ],
  imports: [
    BrowserModule,
    RSCalendarModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    ),
    
    
    
  ],
  
  providers: [],
  bootstrap: [AppComponent, ]
})
export class AppModule { }
