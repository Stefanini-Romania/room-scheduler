import { TestBed, async, inject } from '@angular/core/testing';
import { HttpClient, HttpClientModule, HttpRequest, HttpParams } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { MockBackend, MockConnection } from '@angular/http/testing';
import { Http, BaseRequestOptions, Response, ResponseOptions, RequestMethod } from '@angular/http';
import { AuthService} from './../../auth/shared/auth.service';
//import { environment } from '../../../environments/environment';

describe(`AuthService`, () => {

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        HttpClientTestingModule
      ],
      providers: [AuthService]
    });
  });

  afterEach(inject([HttpTestingController], (backend: HttpTestingController) => {
    backend.verify();
  }));

  it(`should send an expected login request`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', 'admin123456').subscribe();

      backend.expectOne((req: HttpRequest<any>) => {
          //const url = environment.apiUrl + '/api/auth/login';
          const body = JSON.stringify({loginName: 'admin@stefanini.com', password: 'admin123456'});

        return req.url === 'auth/login'        
          && req.method === 'POST'
          && req.headers.get('Content-Type') === 'application/json'
          && req.body.get('loginName') === 'admin@stefanini.com'
          && req.body.get('password') === 'admin123456';
      }, `POST to 'auth/login' with loginName and password`);
  })));

  it(`should emit 'false' for 400 Bad Request`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', 'admin123456').subscribe((next) => {
        expect(next).toBeFalsy();
      });

      backend.expectOne('auth/login').flush(null, { status: 400, statusText: 'Bad Request' });
  })));

  it(`should emit 'true' for 200 Ok`, async(inject([AuthService, HttpTestingController],
    (auth: AuthService, backend: HttpTestingController) => {
      auth.authenticate('admin@stefanini.com', 'admin123456').subscribe((next) => {
        expect(next).toBeTruthy();
      });

      backend.expectOne('auth/login').flush(null, { status: 200, statusText: 'Ok' });
  })));

});




// describe('AuthServiceTest', () => {
//   let subject: AuthService = null;
//   let backend: MockBackend = null;

//   beforeEach(inject([AuthService, MockBackend], (authService: AuthService, mockBackend: MockBackend) => {
//     subject = authService;
//     backend = mockBackend;
//   }));

//   backend.connections.subscribe((connection: MockConnection) => {
//     expect(connection.request.method).toEqual(RequestMethod.Post);
//     expect(connection.request.url).toEqual('/login');
//     expect(connection.request.text()).toEqual(JSON.stringify({ username: 'admin@stefanini.com', password: 'admin123456' }));
//     expect(connection.request.headers.get('Content-Type')).toEqual('application/json');
  
    
//   });



//   it('#login should call endpoint and return it\'s result', (done) => {
//     backend.connections.subscribe((connection: MockConnection) => {
//       let options = new ResponseOptions({
//         body: JSON.stringify({ success: true })
//       });
//       connection.mockRespond(new Response(options));
//     });

//     subject
//     .authenticate({ loginName: 'admin@stefanini.com', password: 'admin123456' })
//     .subscribe((response) => {
//       expect(response.json()).toEqual({ success: true });
//       done();
//     });
//   });
// });