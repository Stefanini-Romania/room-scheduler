import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {HttpClientModule, HttpClient} from '@angular/common/http';
import {RouterModule, Routes} from '@angular/router';
import {TranslateModule,TranslateLoader, MissingTranslationHandler, MissingTranslationHandlerParams} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastrModule} from 'ngx-toastr';

import {AppComponent} from './app.component';
import {CoreModule} from './core/core.module';
import {FormsModule} from '@angular/forms';
import {CalendarsModule} from './calendars/calendars.module';
import {SharedModule} from './shared/shared.module';
import {AuthService} from './auth/shared/auth.service';
// import {AuthModule} from './auth/auth.module';
import {UsersModule} from './users/users.module';
import {RoomModule} from './rooms/room.module';
import {RoomService} from './rooms/shared/room.service';
import {PageNotFoundComponent} from "./page-not-found.component";
import {DialogService} from './shared/services/dialog.service';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

export class MyMissingTranslationHandler implements MissingTranslationHandler {
    handle(params: MissingTranslationHandlerParams) {
        console.log(`Missing translation "${params.key}" for language ${params.translateService.currentLang}`);
        return params.key;
    }
}

const routes: Routes = [
    // default route
    {
        path: '',
        redirectTo: '/calendar',
        pathMatch: 'full'
    },

    // Not Found
    {
        path: '**', component: PageNotFoundComponent
    },
];

@NgModule({
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        NgbModule.forRoot(),
        RouterModule.forRoot(routes),
        BrowserAnimationsModule, // required animations module
        ToastrModule.forRoot(), // ToastrModule added
        TranslateModule.forRoot({
            missingTranslationHandler: { provide: MissingTranslationHandler, useClass: MyMissingTranslationHandler },
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),

        CoreModule,
        SharedModule,
        CalendarsModule,
        RoomModule,
        UsersModule
       

    ],

    providers: [RoomService, AuthService, DialogService],
    declarations: [PageNotFoundComponent, AppComponent],
    exports: [],
    bootstrap: [AppComponent]
})

export class AppModule {
}
