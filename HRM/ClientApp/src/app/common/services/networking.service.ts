import { Injectable, Inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, Subject, forkJoin } from 'rxjs';
import { ModalService } from './modal.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Role } from '../constants/constants';

@Injectable()

export class NetworkingService {

  private subject = new Subject<boolean>();

  public httpOptions = {
    headers: new HttpHeaders(
      {
        'Authorization': this.getAccessToken(),
        'Content-Type': 'application/json; charset=utf-8'
      }
    )
  };

  constructor(
    private cookie: CookieService,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private modalService: ModalService,
    private router: Router,
    private translate: TranslateService
  ) { }

  // SET OBSERVABLE WHEN STATE CHANGES
  setLoading(isShow) {
    this.subject.next(isShow);
  }

  // GET OBSERVABLE WHEN STATE CHANGES
  getLoading(): Observable<boolean> {
    return this.subject.asObservable();
  }

  // MAKE REQUEST OBJECT
  makeObjectRequest(url, body?, option?): object {
    var obj = new Object();
    obj["url"] = url;
    obj["body"] = body;
    obj["option"] = option;

    return obj;
  }

  // MAKE MULTIPLE REQUEST
  multipleRequests(requests, next?, error?) {

    // Get Token Storage
    let tokenStorage = this.getToken();

    // Token not exist
    if (tokenStorage === null) {
      this.openModalError();
      return;
    }

    // Check Token Valid
    if (this.isTokenExpired(tokenStorage.accessTokenExpires)) {

      const token = {
        AccessToken: tokenStorage.accessToken,
        RefreshToken: tokenStorage.refreshToken
      }

      // Re-call API
      this.httpOptions = {
        headers: new HttpHeaders(
          {
            'Authorization': this.getAccessToken(),
            'Content-Type': 'application/json; charset=utf-8'
          }
        )
      };

      this.http.post(this.baseUrl + "api/Auth/Refresh", token, this.httpOptions).subscribe(
        res => {
          // Re-save cookie new token
          this.cookie.set("token", JSON.stringify(res["token"]));

          // Call multiple requests
          this.forkJoin(requests, next, error);
        },
        (error: HttpErrorResponse) => {
          console.log("Refresh Token Error: ", error.message);
          this.openModalError();
        }
      );
    }
    else {
      // Call multiple requests
      this.forkJoin(requests, next, error);
    }
  }

  // MAKE REQUEST MULTIPLE
  private forkJoin(requests: object[], next?, error?) {
    const self = this;
    let arrReqs = [];


    // Reset accessToken
    self.httpOptions = {
      headers: new HttpHeaders(
        {
          'Authorization': self.getAccessToken(),
          'Content-Type': 'application/json; charset=utf-8'
        }
      )
    };

    requests.forEach(function (obj) {
      // Check body
      if (obj["body"] === undefined) {
        const req = self.http.get(obj["url"], self.httpOptions);
        arrReqs.push(req);
      }
      else {
        const req = self.http.post(obj["url"], obj["body"], self.httpOptions);
        arrReqs.push(req);
      }
    })

    forkJoin(arrReqs).subscribe(
      res => {
        this.setLoading(false);
        next(res);
      },
      (err: HttpErrorResponse) => {
        this.setLoading(false);
        console.log("Error: ", err.message);
        error(err);
      }
    )
  }

  // OVERRIDE METHOD GET FROM HttpClient
  get(url: string, options?: {
    headers?: HttpHeaders | {
      [header: string]: string | string[];
    };
    observe?: 'body';
    params?: HttpParams | {
      [param: string]: string | string[];
    };
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
  }, next?, error?) {

    // Show Loading
    this.setLoading(true);

    // Check options
    if (options == null) {
      options = this.httpOptions;
    }

    if (next == undefined && error == undefined) {
      return this.http.get(url, options);
    }
    else {
      // Override method get
      this.http.get(url, options).subscribe(
        res => {
          // Hide Loading
          this.setLoading(false);

          // Return Response
          next(res);
        },
        (err: HttpErrorResponse) => {
          // Hide Loading
          this.setLoading(false);

          // Handle Error
          this.handleError(err, url, null, next, error);
        }
      );
    }
  }

  // OVERRIDE METHOD POST FROM HttpClient
  post<T>(url: string, body: any | null, options?: {
    headers?: HttpHeaders | {
      [header: string]: string | string[];
    };
    observe?: 'body';
    params?: HttpParams | {
      [param: string]: string | string[];
    };
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
  }, next?, error?) {

    // Show Loading
    this.setLoading(true);

    // Check options
    if (options == null) {
      options = this.httpOptions;
    }

    if (next == undefined && error == undefined) {
      return this.http.post(url, body, options);
    }
    else {
      // override method post
      this.http.post(url, body, options).subscribe(
        res => {
          // Hide Loading
          this.setLoading(false);

          // Return Response
          next(res);
        },
        (err: HttpErrorResponse) => {
          // Hide Loading
          this.setLoading(false);

          // Handle Error
          this.handleError(err, url, body, next, error);
        }
      );
    }
  }

  // HANDLE REQUEST RESPONSE ERROR
  handleError(err, url, body?, next?, error?) {
    const self = this;

    if (err.status === 401 && err.headers.has('Token-Expired')) {
      // Get refresh token from client
      let tokenStorage = this.cookie.get("token");

      if (tokenStorage === "") {
        // Session expires. Back to login
        this.openModalError();
      }

      // Call API Refresh Token
      const token = {
        AccessToken: JSON.parse(tokenStorage).accessToken,
        RefreshToken: JSON.parse(tokenStorage).refreshToken
      }

      // Refresh Token
      self.refreshToken(token, url, body, next);
      
    }
    else {
      // error
      if (error) {
        error(err);
      }
    }
  }

  // REFRESH TOKEN
  refreshToken(token, url, body?, next?) {
    this.http.post(this.baseUrl + "api/Auth/Refresh", token, this.httpOptions).subscribe(
      res => {
        // Re-save cookie new token
        this.cookie.set("token", JSON.stringify(res["token"]));

        // Re-call API
        this.httpOptions = {
          headers: new HttpHeaders(
            {
              'Authorization': this.getAccessToken(),
              'Content-Type': 'application/json; charset=utf-8'
            }
          )
        };

        // Define GET or POST
        const req = (body === null) ? this.http.get(url, this.httpOptions) : this.http.post(url, body, this.httpOptions);
        req.subscribe(
          res => {
            next(res);
          }
        );
      },
      (error: HttpErrorResponse) => {
        console.log("Refresh Token Error: ", error.message);
        this.openModalError();
      }
    );
  }

  // GET ACCESS TOKEN FROM COOKIE
  getAccessToken(): string {
    return (this.cookie.get("token") === "") ? null : "Bearer " + JSON.parse(this.cookie.get("token")).accessToken;
  }

  // GET ACCESS TOKEN FROM COOKIE
  getToken(): any {
    return (this.cookie.get("token") === "") ? null : JSON.parse(this.cookie.get("token"));
  }

  // GET ACCOUNT FROM COOKIE. Use getAccount().userId or getAccount().userName
  getAccount(): any {
    return (this.cookie.get("account") === "") ? null : JSON.parse(this.cookie.get("account"));
  }

  // SHOW OPEN SESSION EXPIRED. BACK TO LOGIN
  openModalError() {
    const self = this;
    this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('Modal.Message.SessionExp'), this.translate.instant('Login.BtnLogin'), function () {
      self.router.navigateByUrl("/Login");
    });
  }

  // CHECK TOKEN EXPIRES
  isTokenExpired(expires): boolean {
    let curent = new Date();
    let expired = new Date(expires);

    return (expired > curent) ? false : true;
  }

  // DELETE ALL COOKIES
  deleteCookies() {
    this.cookie.delete("token");
    this.cookie.delete("account");
  }
} 
