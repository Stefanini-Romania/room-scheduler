import { TestBed, async, inject } from '@angular/core/testing';
import { HttpClient, HttpClientModule, HttpRequest, HttpParams } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { MockBackend, MockConnection } from '@angular/http/testing';
import { Http, BaseRequestOptions, Response, ResponseOptions, RequestMethod } from '@angular/http';
import { AuthService} from './../../auth/shared/auth.service';
import { DialogService } from './../../shared/services/dialog.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {TranslateModule} from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
//import { environment } from '../../../environments/environment';

describe(`AuthService`, () => {
  let subject: AuthService = null;
  let backend: MockBackend = null;

 

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        HttpClientTestingModule, 
        NgbModule.forRoot(),
        TranslateModule.forRoot(),
        RouterTestingModule.withRoutes([])
        ],
      providers: [AuthService, DialogService, NgbModal, MockBackend, HttpClient, HttpTestingController]
    });
  });

  beforeEach(inject([AuthService, MockBackend], (authService: AuthService, mockBackend: MockBackend) => {
    subject = authService;
    backend = mockBackend;
    
  }));

  it(`should send an expected login request`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', 'admin123456').subscribe();

      backend.expectOne((req: HttpRequest<any>) => {
          //const url = environment.apiUrl + '/api/auth/login';
          const body = JSON.stringify({success: true});
          // const body = new HttpParams({fromString: req.body});

        return req.url === 'auth/login'        
          && req.method === 'POST'
          && req.headers.get('Content-Type') === 'application/json'
          && req.body.get('loginName') === 'admin@stefanini.com'
          && req.body.get('password') === 'admin123456';
      }, `POST to 'auth/login' with loginName and password`);
  })));
  it('isLoggedIn should return false after creation', inject([AuthService], (service: AuthService) => {
    expect(service.isLoggedIn()).toBeFalsy();
  }));

  it(`should emit 'true' for 200 Ok`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', 'admin123456').subscribe((next) => {
      backend.expectOne('auth/login').flush({status: 200, statusText: 'Ok'});
      });
  })));

  it(`should emit 'false' for 400 Bad Request`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', 'admin123456').subscribe((next) => {
      backend.expectOne('auth/login').flush({ status: 400, statusText: 'Bad Request' });
      });

  })));


  afterEach(inject([HttpTestingController], (backend: HttpTestingController) => {
    backend.verify();
  }));



 });






