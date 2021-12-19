import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";

import { LoginService } from '../services/login.service';
import { HttpErrorResponse } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { NetworkingService } from '../../../common/services/networking.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {
  @ViewChild('titleResetElement') titleResetElement: ElementRef;
  @ViewChild('msgResetElement') msgResetElement: ElementRef;
  @ViewChild('btnResetElement') btnResetElement: ElementRef;

  submitted = false;

  langActive = "en";

  langOptions = [
    {
      id: "en",
      des: "English"
    },
    {
      id: "jp",
      des: "日本語"
    },
    {
      id: "vn",
      des: "Tiếng Việt"
    }
  ];

  // login form
  loginFormGroup: FormGroup;
  isShowLoginForm = true;
  isShowLoginError = false;
  msgLoginError = "";

  // forgetForm
  forgetFormGroup: FormGroup;
  isShowForgetForm = false;

  // resetSuccessForm
  isShowResetSuccessForm = false;
  titleReset = "";
  msgReset = "";

  // image logo
  imgLogo = {
    url: '../assets/images/logo-h.png',
    show: false
  }

  constructor(
    private formBuilder: FormBuilder,
    private loginService: LoginService,
    private router: Router,
    private cookie: CookieService,
    private translate: TranslateService,
    private networkService: NetworkingService
  ) {
    const lang = this.cookie.get("lang") === "" ? "en" : this.cookie.get("lang");
    this.langActive = lang;
    this.translate.setDefaultLang(lang);
    this.translate.use(lang);
  }

  ngOnInit() {
    this.loginFormGroup = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', Validators.required]
    });

    this.forgetFormGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]]
    });
  }

  get loginData() { return this.loginFormGroup.controls; }
  get forgetData() { return this.forgetFormGroup.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.loginFormGroup.invalid) {
      return;
    }

    // Set Loading
    this.networkService.setLoading(true);

    // Call API
    this.loginService.login(this.loginFormGroup.value).subscribe(
      res => {
        this.networkService.setLoading(false);
        if (res !== null) {
          this.cookie.set("token", JSON.stringify(res.token));
          this.cookie.set("account", JSON.stringify(res.account));
          this.router.navigateByUrl('/Dashboard');
        }
        else {
          this.isShowLoginError = true;
        }
      },
      (error: HttpErrorResponse) => {
        this.networkService.setLoading(false);
        this.isShowLoginError = true;
        this.msgLoginError = error.error.message;
      }
    );
  }

  onChangeLanguage(event) {
    this.langActive = event.target.value;
    this.cookie.set("lang", event.target.value);
    this.translate.use(event.target.value);
  }
}
