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
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.css']
})
export class AccountListComponent implements OnInit {
  listAccounts: any;
  p: number = 1;
  pageCount = 0;

  constructor(
    private translate: TranslateService,
    private networkSevice: NetworkingService,
    @Inject('BASE_URL') private _baseUrl: string,
    private modalService: ModalService) {

  }

  ngOnInit(): void {
    this.getAccounts();
  }

  getAccounts() {
    const self = this;

    this.networkSevice.get(this._baseUrl + "api/Account/GetAccountList", null, function (res) {
      self.listAccounts = res.listAccounts;
      self.pageCount = res.pageCount;
    });
  }

  onDelete(userName) {
    this.modalService.openModalConfirm(this.translate.instant('Modal.Title.Confirm'), this.translate.instant('AccountDelete.MsgConfirm'),
      Ok => {
        this.networkSevice.post<any>(this._baseUrl + "api/Account/Delete", JSON.stringify(userName), null,
          res => {
            if (res.statusCode == 1) {
              this.modalService.openModalSuccess(this.translate.instant('Modal.Title.Success'), this.translate.instant('AccountDelete.MsgSuccess'), "OK", Ok => {
                this.ngOnInit();
              });
            }
          },
          err => {
            this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('AccountDelete.MsgError'), "OK");
          });
      });
  }
}
