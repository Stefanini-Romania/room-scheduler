import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import {TranslateModule} from '@ngx-translate/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule, ToastContainerModule } from 'ngx-toastr';
import {CoreModule} from '../core.module';
import { Router, ActivatedRoute } from '@angular/Router';

import {RsHeaderComponent} from './rs-header.component';
import {AuthModule} from '../../auth/auth.module';
import {SharedModule} from '../../shared/shared.module';

describe('RsHeaderComponent (templateUrl)', ()=>{
    let component: RsHeaderComponent;
    let fixture: ComponentFixture<RsHeaderComponent>;
    let de: DebugElement;
    let el: HTMLElement;
    let routerStub;
   

    //async BeforeEach
    beforeEach(async(() => {
         routerStub = {navigate: jasmine.createSpy('navigate')}
        TestBed.configureTestingModule({
            imports: [TranslateModule.forRoot(),
                    NgbModule,
                    AuthModule,
                    SharedModule,
                    HttpClientModule,
                    HttpModule,
                    RouterTestingModule,
                    ToastrModule.forRoot(),
                    ToastContainerModule,
                    CoreModule
                     ],
            declarations: [], // declare the test component
            providers: [{ provide: Router, useClass: routerStub }]
        })
        .compileComponents();  // compile template and css
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(RsHeaderComponent);

        component = fixture.componentInstance; // RsHeaderComponent test instance

        // query for the title <>, a or div by CSS element selector
        de = fixture.debugElement.query(By.css('div'));
        de = fixture.debugElement.query(By.css('btn'));
        de = fixture.debugElement.query(By.css('b'));
        el = de.nativeElement;
    });

    // it('should redirect to login', () => {
    //     fixture.detectChanges();
    //     component.redirectToLogin();
    //     expect(el).toHaveBeenCalledWith(['/login']);
    // });
});