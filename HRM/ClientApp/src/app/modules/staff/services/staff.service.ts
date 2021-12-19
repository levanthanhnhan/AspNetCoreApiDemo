import { Injectable, Inject } from '@angular/core';
import { HttpHeaders } from "@angular/common/http";
import { CookieService } from 'ngx-cookie-service';
import { StaffModel } from '../models/staff-model';
import { NetworkingService } from '../../../common/services/networking.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ModalService } from '../../../common/services/modal.service';
import { ok } from 'assert';

@Injectable({
    providedIn: 'root'
})
export class StaffService {
    httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' })
    };
    constructor(
        public cookie: CookieService,
        private networkSevice: NetworkingService,
        private translate: TranslateService,
        private router: Router,
        @Inject('BASE_URL')
        private _baseUrl: string,
        private modalService: ModalService
    ) { }

    update(model: StaffModel) {
        model.sex = Number(model.sex);
        model.departmentId = Number(model.departmentId);
        model.positionId = Number(model.positionId);

        let self = this;
        this.networkSevice.post(this._baseUrl + "api/Staff/Update", model, null, function (res) {
            if (res['status'] == 'SUCCESS') {
              self.modalService.openModalSuccess(self.translate.instant('Staff-up.UpdateSuccess'), self.translate.instant('Staff-up.ContentUpdateSuccess'), "OK", ok => {
                self.router.navigateByUrl('/Staff/List');
              });
            } else {
              self.modalService.openModalError(self.translate.instant('Staff-up.UpdateFailed'), self.translate.instant('Staff-up.ContentUpdateFailed'), "OK");
            }
        });
    }

    register(model: StaffModel) {
      model.sex = Number(model.sex);

      let self = this;
      this.networkSevice.post(this._baseUrl + "api/Staff/Register", model, null, function (res) {
        if (res['status'] == 'SUCCESS') {
          self.modalService.openModalSuccess(self.translate.instant('Staff-up.UpdateSuccess'), self.translate.instant('StaffRegister.Message.Success'), "OK", ok => {
            self.router.navigateByUrl('/Staff/List');
          });
        } else {
          self.modalService.openModalError(self.translate.instant('Staff-up.UpdateFailed'), self.translate.instant('StaffRegister.Message.Failed'), "OK");
        }
      });
    }
}
