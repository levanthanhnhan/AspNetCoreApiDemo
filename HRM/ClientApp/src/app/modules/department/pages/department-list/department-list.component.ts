import { Component, OnInit, Inject, ElementRef, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NetworkingService } from '../../../../common/services/networking.service';
import { ModalService } from '../../../../common/services/modal.service';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
  styleUrls: ['./department-list.component.scss']
})

export class DepartmentListComponent implements OnInit {
  listDepartment: any;
  isChange = false;
  @ViewChild('txtSearch') txtSearch: ElementRef;

  constructor(
    private translate: TranslateService,
    private networkSevice: NetworkingService,
    @Inject('BASE_URL') private _baseUrl: string,
    private modalService: ModalService) {

  }

  ngOnInit(): void {
    this.getDepartment();
  }

  getDepartment() {
    const self = this;

    this.networkSevice.get(this._baseUrl + "api/Department", null, function (res) {
      self.listDepartment = res;
    });
  }

  search() {
    const self = this;
    const params = {
      DepartmentName: this.txtSearch.nativeElement.value.replace(/ +(?= )/g, '').trim()
    }
    this.networkSevice.post(this._baseUrl + "api/Department/Search", params, null, function (res) {
      self.listDepartment = res;
    });
  }

  clear() {
    this.isChange = false;
    this.txtSearch.nativeElement.value = '';
    this.getDepartment();
  }

  onSearchChange() {
    if (this.txtSearch.nativeElement.value.length == 0) {
      this.isChange = false;
    } else {
      this.isChange = true;
    }
  }
  onDelete(id) {
    this.modalService.openModalConfirm(this.translate.instant('Modal.Title.Confirm'), this.translate.instant('Department.ConfirmDel'),
      Ok => {
        this.networkSevice.post<any>(this._baseUrl + "api/Department/Delete", JSON.stringify(id), null,
          res => {
            if (res._statusCode == 1) {
              this.modalService.openModalSuccess(this.translate.instant('Modal.Title.Success'), this.translate.instant('Department.SuccessDel'), "OK", Ok => {
                this.ngOnInit();
              });
            } else {
              this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('Department.ErrorExistStaff'), "OK");
            }
          },
          err => {
            this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('Department.ErrorDel'), "OK");
          });
      });
  }
}
