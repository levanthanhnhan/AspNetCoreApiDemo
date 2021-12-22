import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NetworkingService } from '../../../../common/services/networking.service';
import { AccountModel } from "../../../login/models/account-model";
import { ComparePassword } from "../../services/customvalidator.validator";
import { TranslateService } from "@ngx-translate/core";
import { ModalService } from '../../../../common/services/modal.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-register',
  templateUrl: './account-register.component.html',
  styleUrls: ['./account-register.component.css']
})
export class AccountRegisterComponent implements OnInit {
  regisForm: FormGroup;
  lstStaff: any;
  lstRole: any;
  submitted = false;
  selected = null;
  status: any;

  constructor(
    private networkService: NetworkingService,
    private translate: TranslateService,
    private formBuilder: FormBuilder,
    private modalService: ModalService,
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
  }

  get regisData() { return this.regisForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.regisForm.invalid) {
      return;
    }

    var staff = this.regisForm.get("staff").value;
    let reqUserObj = new AccountModel();
    reqUserObj.username = this.regisForm.get("username").value;
    reqUserObj.email = this.regisForm.get("username").value + "@company.com";
    reqUserObj.roleId = Number(this.regisForm.get("role").value);
    reqUserObj.password = this.regisForm.get("password").value;
    reqUserObj.staffId = staff.id;

    this.getResRegister(reqUserObj);
  }

  getAllStaff() {
    let self = this;
    return this.networkService.get(this._baseUrl + "api/Account", null, function (res) {
      self.lstStaff = res['_listStaff'];
      self.lstRole = res['_listRole'];
      let iStaff = self.lstStaff.filter(staff => staff.companyEmail != "");
      this.lstStaff = iStaff;
    });
  }

  getResRegister(reqModal) {
    let self = this;
    this.networkService.post(this._baseUrl + "api/Account/Create", reqModal, null, function (res) {
      status = res['_statusCode'];

      if (status == '1') {
        self.modalService.openModalSuccess(self.translate.instant('Register.SuccessModalTitle'), self.translate.instant('Register.Message.CreateSuccess'), 'OK', ok => {
          self.router.navigateByUrl('/Account/List');
        });
      }
      else {
        self.modalService.openModalError(self.translate.instant('Register.ErrorModalTitle'), self.translate.instant('Register.Message.CreateFail'), 'OK');
      }
    });
  }

  onback() {
    this.router.navigateByUrl('/Account/List');
  }
}
