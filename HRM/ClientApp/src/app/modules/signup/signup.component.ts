import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NetworkingService } from '../../common/services/networking.service';
import { AccountModel } from "../login/models/account-model";
import { TranslateService } from "@ngx-translate/core";
import { Router } from '@angular/router';
import { ModalService } from '../../common/services/modal.service';
import { ComparePassword } from '../account/services/customvalidator.validator';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignUpComponent implements OnInit {
  regisForm: FormGroup;
  submitted = false;
  selected = null;
  status: any;

  constructor(
    private networkService: NetworkingService,
    private translate: TranslateService,
    private formBuilder: FormBuilder,
    private modalService: ModalService,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private _baseUrl: string) { }

  ngOnInit(): void {
    this.regisForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(50)]],
    });
  }

  get regisData() { return this.regisForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.regisForm.invalid) {
      return;
    }

    let reqObj = {
      userName: this.regisForm.get("username").value,
      password: this.regisForm.get("password").value
    }

    this.getResRegister(reqObj);
  }

  getResRegister(reqObj) {
    let self = this;
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' })
    };

    this.http.post<any>(this._baseUrl + "api/SignUp", JSON.stringify(reqObj.userName), httpOptions).subscribe(res => {
      self.router.navigateByUrl('/Login');
    });


    //this.networkService.post(this._baseUrl + "api/Account/SignUp", reqModal, null, function (res) {
    //  self.modalService.openModalSuccess(self.translate.instant('Register.SuccessModalTitle'), self.translate.instant('Register.Message.CreateSuccess'), 'OK', ok => {
    //    self.router.navigateByUrl('/Login');
    //  });
    //});
  }

  onback() {
    this.router.navigateByUrl('/Login');
  }
}
