import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import {RouterModule, Routes} from '@angular/router';
import {AppComponent} from './app.component';
import {CoreModule} from './core/core.module';
import {FormsModule} from '@angular/forms';
import {CalendarsModule} from './calendars/calendars.module';
import {SharedModule} from './shared/shared.module';
import {AuthService} from './auth/shared/auth.service';
import {AuthModule} from './auth/auth.module';
import {RoomModule} from './rooms/room.module';
import {RoomService} from './rooms/shared/room.service';

const routes: Routes = [
    // default route
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    },

    // Not Found
    {
        path: '**', redirectTo: '/'
    },
];

@NgModule({
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        CoreModule,
        SharedModule,
        CalendarsModule,
        RouterModule.forRoot(routes),
        RoomModule,
        AuthModule,

    ],

    providers: [RoomService, AuthService],
    declarations: [AppComponent],
    exports: [],
    bootstrap: [AppComponent]
})

export class AppModule {
}
