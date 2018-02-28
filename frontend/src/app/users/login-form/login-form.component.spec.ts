import { TestBed, async, inject } from '@angular/core/testing';
import { HttpClient, HttpClientModule, HttpRequest, HttpParams } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService} from './../../auth/shared/auth.service';
import { environment } from '../../../environments/environment';

describe(`AuthService`, () => {

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        HttpClientTestingModule
      ],
      providers: [
        AuthService
      ]
    });
  });

  afterEach(inject([HttpTestingController], (backend: HttpTestingController) => {
    backend.verify();
  }));

  it(`should send an expected login request`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', '123456').subscribe();

      backend.expectOne((req: HttpRequest<any>) => {
          const url = environment.apiUrl + '/api/auth/login';
          const body = JSON.stringify({loginName: loginName, password: password});

        return req.url === url
          && req.method === 'POST'
          && req.headers.get('Content-Type') === 'application/json'
          && body.get('loginName') === 'admin@stefanini.com'
          && body.get('password') === '123456';
      }, `POST to 'api/auth/login' with form-encoded loginName and password`);
  })));

  it(`should emit 'false' for 401 Unauthorized`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', '123456').subscribe((next) => {
        expect(next).toBeFalsy();
      });

      backend.expectOne('auth/login').flush(null, { status: 401, statusText: 'Unauthorized' });
  })));

  it(`should emit 'true' for 200 Ok`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', '123456').subscribe((next) => {
        expect(next).toBeTruthy();
      });

      backend.expectOne('api/auth/login').flush(null, { status: 200, statusText: 'Ok' });
  })));

});