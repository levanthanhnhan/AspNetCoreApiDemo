import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AccountModel } from "../models/account-model";
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})

export class LoginService {

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' })
  };

  constructor(public cookie: CookieService, private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  login(model: AccountModel) {
    return this.httpClient.post<any>(this.baseUrl + "api/Login", model, this.httpOptions);
  }

  resetPassword(email) {
    return this.httpClient.post<any>(this.baseUrl + "api/ResetPassword", JSON.stringify(email), this.httpOptions);
  }

  getAccessToken(): string {
    if (this.cookie.get("token") === "") {
      return null
    }

    return "Bearer " + JSON.parse(this.cookie.get("token")).accessToken;
  }
}
