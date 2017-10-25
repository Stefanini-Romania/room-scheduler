import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { FormsModule } from '@angular/forms';
import { CalendarsModule } from './calendars/calendars.module';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './auth/auth.module';
import { AuthService } from './auth/shared/auth.service';

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
        HttpClientModule,
        FormsModule,
        CoreModule,
        SharedModule,
        AuthModule,
        CalendarsModule,
        RouterModule.forRoot(routes),
    ],
  
    providers: [AuthService],
    declarations: [AppComponent],
    exports: [],
    bootstrap: [AppComponent]
})

export class AppModule { }
