import { Component, OnInit, Inject, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ExcelService } from '../../services/excel.service';
import { TranslateService } from '@ngx-translate/core';
import { NetworkingService } from '../../../../common/services/networking.service';
import { CookieService } from 'ngx-cookie-service';
import { ModalService } from '../../../../common/services/modal.service';

@Component({
  selector: 'app-staff-list',
  templateUrl: './staff-list.component.html',
  styleUrls: ['./staff-list.component.scss']
})

export class StaffListComponent implements OnInit {

  p: number = 1;
  listStaff: any;
  listRelationStaff: any;
  closeResult: string;
  masterSelected: boolean;
  checkedList: any;
  result: any;
  isModalOpened = false;
  pageCount = 0;
  accepted: boolean;
  isAdminRole: boolean;
  searchSkillList: any = [];
  satffIdList: any;
  detailFlag: boolean;
  account: any;

  constructor(
    private translate: TranslateService,
    private networkSevice: NetworkingService,
    private cookie: CookieService,
    @Inject('BASE_URL') private _baseUrl: string,
    private modalService: ModalService) {
      this.masterSelected = false;
      this.account = this.networkSevice.getAccount();
  }

  ngOnInit(): void {
    const self = this;
    this.getData(function () {
      self.getStaff();
    });
    this.detailFlag = false;
  }

  getData(next?) {
    const self = this;

    this.networkSevice.get(this._baseUrl + "api/Staff", null, function (res) {
      self.listStaff = res;
      next();
    });
  }

   getStaff(params = undefined) {
    this.p = 1;
    const self = this;

    if (params === undefined) {
      params = {
        DepartmentId: 0,
        PositionId: 0,
        StaffName: ""
      }
    }

    // Call API Search
    this.networkSevice.post(this._baseUrl + "api/Staff/GetRelationStaffs", params, null, function (res) {
      self.listRelationStaff = res.list;
      self.pageCount = res.pageCount;
    });
  }

  @ViewChild('list_name') listStaffName: ElementRef;
  clearSearch(params) {
    this.p = 1;
    this.getStaff(params);
    this.checkedList = "[]";
    this.listRelationStaff = [];
    let el: HTMLElement = this.listStaffName.nativeElement;
    el.click();
    this.listStaffName.nativeElement.checked = false;
  }

  deleteClick() {
    this.modalService.openModalConfirm(this.translate.instant('Modal.Title.Confirm'), this.translate.instant('Modal.Message.ConfirmDel'),
      Ok => {
        const self = this;
        self.checkedList;

        if (self.checkedList == undefined || self.checkedList == "[]") {
          self.modalService.openModalError(this.translate.instant('Modal.Title.Warning'), this.translate.instant('StaffList.Message.Warning1'), this.translate.instant('Modal.Action.Ok'), Ok => { });
          return;
        }
        this.networkSevice.post(this._baseUrl + "api/Staff/DeleteStaffs", this.checkedList, null, function (res) {
          self.result = res;
          if (self.result > 0) {
            // Get account storage
            const currentUserId = self.networkSevice.getAccount().userId;

            // Remove token storage if delete me
            const arrs = JSON.parse(self.checkedList);
            Object.keys(arrs).map(function (idx) {
              if (arrs[idx] == currentUserId) {
                self.networkSevice.deleteCookies();
              }
            });

            self.modalService.openModalDelete(self.translate.instant('Modal.Title.Success'), self.translate.instant('StaffList.Message.Success'), self.result, self.translate.instant('Modal.Action.Ok'), Ok => {
              self.checkedList = undefined;
              self.getStaff();
            });
          } else {
            self.modalService.openModalError(self.translate.instant('Modal.Title.Warning'), self.translate.instant('StaffList.Message.Warning2'), self.translate.instant('Modal.Action.Ok'), Ok => { });
          }
        });
      },
      Cancel => {
      })
  }

  addZeroToId(id: number, maxLength: number): string {
    var returnString = id.toString();
    while (returnString.length < maxLength) {
      returnString = '0' + returnString;
    }
    return returnString;
  }

  checkUncheckAll() {
    for (var i = 0; i < this.listRelationStaff.length; i++) {
      this.listRelationStaff[i].isSelected = this.masterSelected;
    }
    this.getCheckedItemList();
  }

  isAllSelected() {
    this.masterSelected = this.listRelationStaff.every(function (item: any) {
    })
    this.getCheckedItemList();
  }

  getCheckedItemList() {
    this.checkedList = [];
    for (var i = 0; i < this.listRelationStaff.length; i++) {
      if (this.listRelationStaff[i].isSelected)
        this.checkedList.push(this.listRelationStaff[i].id);
    }
    this.checkedList = JSON.stringify(this.checkedList);
  }
}
