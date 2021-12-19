import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { AccountModel } from "../../login/models/account-model";
import { LoginService } from '../../login/services/login.service';
import { NetworkingService } from '../../../common/services/networking.service';

@Injectable({
  providedIn: 'root'
})
export class RegisService {

  constructor(
    private _loginService: LoginService,
    private httpClient: HttpClient,
    private networkService: NetworkingService,
    @Inject('BASE_URL') private _baseUrl: string
  ) { }

  httpOptions = {
    headers: new HttpHeaders(
      {
        'Authorization': this._loginService.getAccessToken(),
        'Content-Type': 'application/json; charset=utf-8'
      })
  };

  regis(model: AccountModel) {
    let data: any;
    try {
      this.networkService.post(this._baseUrl + "api/Account/Create", model, null, function (res) {
        data = res;
      });
    } catch (e) {
      throw (e);
    }
    return data;
  }
}
