import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, NgForm } from "@angular/forms";
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { LoginService } from "../../../login/services/login.service";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ModalErrorComponent } from '../../../../common/modals/error/modal-error.component';
import { ModalSuccessComponent } from '../../../../common/modals/success/modal-success.component';
import { NetworkingService } from '../../../../common/services/networking.service';
import { AccountModel } from "../../../login/models/account-model";
import { ComparePassword } from "../../services/customvalidator.validator";
import { TranslateService } from "@ngx-translate/core";
import { ModalService } from '../../../../common/services/modal.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';

@Component({
  selector: 'app-account-edit',
  templateUrl: './account-edit.component.html',
  styleUrls: ['./account-edit.component.scss']
})

export class AccountEditComponent{
  regisForm: FormGroup;
  lstStaff: any;
  lstRole: any;
  submitted = false;
  selected = null;
  status: any;
  account: any;

  constructor(
    private networkService: NetworkingService,
    private translate: TranslateService,
    private formBuilder: FormBuilder,
    private modalService: ModalService,
    private routerActive: ActivatedRoute,
    private router: Router,
    @Inject('BASE_URL') private _baseUrl: string) { }

  ngOnInit(): void {
    this.getAllStaff();
    this.regisForm = this.formBuilder.group({
      username: ['', Validators.required],
      staff: ['', Validators.required],
      role: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(50)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(50)]]
    },
    {
      // Used custom form validator name
      validator: ComparePassword("password", "confirmPassword")
    });

    this.routerActive.paramMap.subscribe((params: ParamMap) => {
      this.getAccountByUserName(params.get('id'))
    });
  }

  get regisData() { return this.regisForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.regisForm.invalid) {
      return;
    }

    let reqUserObj = new AccountModel();
    reqUserObj.username = this.regisForm.get("username").value;
    reqUserObj.email = this.regisForm.get("username").value + "@company.com";
    reqUserObj.roleId = Number(this.regisForm.get("role").value);
    reqUserObj.password = this.regisForm.get("password").value;
    reqUserObj.staffId = this.regisForm.get("staff").value;

    this.updateAccount(reqUserObj);
  }

  getAllStaff() {
    let self = this;
    return this.networkService.get(this._baseUrl + "api/Account", null, function (res) {
      self.lstStaff = res['_listStaff'];
      self.lstRole = res['_listRole'];
      let iStaff = self.lstStaff.filter(staff => staff.companyEmail != "");
      self.lstStaff = iStaff;
    });
  }

  updateAccount(reqModal) {
    let self = this;
    this.networkService.post(this._baseUrl + "api/Account/Update", reqModal, null, function (res) {
      status = res['_statusCode'];

      if (status == '1') {
        self.modalService.openModalSuccess(self.translate.instant('Register.SuccessModalTitle'), self.translate.instant('AccountEdit.MsgSuccess'), 'OK', ok => {
          self.router.navigateByUrl('/Account/List');
        });
      }
      else {
        self.modalService.openModalError(self.translate.instant('Register.ErrorModalTitle'), self.translate.instant('AccountEdit.MsgError'), 'OK');
      }
    });
  }

  getAccountByUserName(userName) {
    const self = this;

    this.networkService.get(this._baseUrl + "api/Account/" + userName, null, function (res) {
      self.account = res.account;
    });
  }

  onback() {
    this.router.navigateByUrl('/Account/List');
  }
}
