// import {Injectable} from '@angular/core';
// import {Headers, Http, RequestOptions} from '@angular/http';
// import {Router} from "@angular/router";
// import {AuthenticationService} from "./services/auth.service";

// import 'rxjs/add/operator/toPromise';
// import {Observable} from 'rxjs/Rx';

// @Injectable()

// export class AppServices {

//   api = '//endpoint/';

//   public options: any;
//   constructor(
//     private http: Http,
//     private router: Router,
//     public authService: AuthenticationService // doesn't work
//  @Inject(forwardRef(() => AuthenticationService)) public authService: AuthenticationService // doesn't work either
//       ) {
//         let head = new Headers({
//       'Authorization': 'Bearer ' + this.authService.token,
//       "Content-Type": "application/json; charset=utf8"
//     });
//     this.options = new RequestOptions({headers: head});
//   }

//   // ====================
//   //    data services
//   // ====================
//   getData(): Promise<any> {
//     return this.http
//       .get(this.api + "/data", this.options)
//       .toPromise()
//       .then(response => response.json() as Array<Object>)
//       .catch((err)=>{this.handleError(err);})
//   }