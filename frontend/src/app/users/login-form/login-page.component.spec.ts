import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import {TranslateModule} from '@ngx-translate/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule, ToastContainerModule } from 'ngx-toastr';
import {CoreModule} from '../../core/core.module';
import { Router, ActivatedRoute } from '@angular/Router';
import { APP_BASE_HREF } from '@angular/common';

import {LoginPageComponent} from './login-page.component';
import {AuthModule} from '../../auth/auth.module';
import {SharedModule} from '../../shared/shared.module';
import {UsersModule} from '../users.module';


describe('LoginPageComponent (templateUrl)', ()=>{
    let component: LoginPageComponent;
    let fixture: ComponentFixture<LoginPageComponent>;
    let de: DebugElement;
    let el: HTMLElement;
    let router;
   

    //async BeforeEach
    beforeEach(async(() => {
         router = {navigate: jasmine.createSpy('navigate')}
        TestBed.configureTestingModule({
            imports: [TranslateModule.forRoot(),
                    NgbModule,
                    AuthModule,
                    SharedModule,
                    HttpClientModule,
                    HttpModule,
                    RouterTestingModule.withRoutes([{ path: 'login', component: LoginPageComponent},
                                                    ]),
                    ToastrModule.forRoot(),
                    ToastContainerModule,
                    CoreModule,
                    UsersModule
                     ],
            declarations: [], // declare the test component
            providers: [{ provide: Router, useClass: router },
                { provide: APP_BASE_HREF, useValue : '/' }]
        })
        .compileComponents();  // compile template and css
    }));

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [ LoginPageComponent ], // declare the test component
          });
        fixture = TestBed.createComponent(LoginPageComponent);
        component = fixture.componentInstance; // LoginPageComponent test instance
        de = fixture.debugElement.query(By.css('login-form'));
        el = de.nativeElement;
    });

    it('should create login page component', () => {
        fixture.detectChanges();
        expect(component).toBeTruthy();
    });

    it('should redirect to login', async() => {
                    
            component.onSuccessfullLogin();
            fixture.detectChanges();
            expect(router.navigate.toHaveBeenCalled);
        
    });
});