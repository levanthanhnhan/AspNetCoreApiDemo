import { Component, OnInit, Inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NetworkingService } from '../../../../common/services/networking.service';
import { ModalService } from '../../../../common/services/modal.service';
import { ParamMap, ActivatedRoute } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Department } from '../../models/department-model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-department-edit',
  templateUrl: './department-edit.component.html',
  styleUrls: ['./department-edit.component.scss']
})

export class DepartmentEditComponent implements OnInit {
  addForm: FormGroup;
  lstDepartment: any;
  lstStaff: any;
  submitted = false;
  accountStorage: any;
  department: any;

  constructor(
    private _location: Location,
    private translate: TranslateService,
    private networkSevice: NetworkingService,
    @Inject('BASE_URL') private _baseUrl: string,
    private _route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private modalService: ModalService) {
    this.accountStorage = this.networkSevice.getAccount();
  }

  get regisData() { return this.addForm.controls; }

  ngOnInit(): void {
    this.department = {};
    this.getAllDepartment();
    this.getAllStaff();
    this._route.paramMap.subscribe((params: ParamMap) => {
      this.getDepartmentById(params.get('id'))
    });
    this.refreshFormValue();
  }

  refreshFormValue() {
    this.addForm = this.formBuilder.group({
      departmentName: [this.department['name'], Validators.required],
      leaderDepartment: [this.department['leaderStaffId'], Validators.required],
      parentDepartmentId: [this.department['parentId'], Validators.required]
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.addForm.invalid) {
      return;
    }

    let departmentModel = new Department();
    departmentModel.id = this.department.id;
    departmentModel.parentId = +this.addForm.get("parentDepartmentId").value;
    departmentModel.name = this.addForm.get("departmentName").value.toUpperCase();
    departmentModel.leaderStaffId = +this.addForm.get("leaderDepartment").value;
    departmentModel.updateId = this.accountStorage.userId;
    let isDepartmentExist = false;

    this.lstDepartment.forEach(function (department) {
      if (department.name == departmentModel.name && department.id != departmentModel.id) {
        isDepartmentExist = true;
      }
    });

    if (!isDepartmentExist) {
      this.networkSevice.post<any>(this._baseUrl + "api/Department/Update", JSON.stringify(departmentModel), null,
        res => {
          if (res._statusCode == 1) {
            this.modalService.openModalSuccess(this.translate.instant('Modal.Title.Success'), this.translate.instant('DepartmentEdit.UpdateSuccess'), "OK",
              ok => {
                this._location.back();
              });
          } else {
            this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('DepartmentEdit.UpdateFail'), "OK");
          }
        }
        ,
        err => {
          this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('DepartmentEdit.UpdateFail'), "OK");
        }
      );
    } else {
      this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('DepartmentEdit.EditFail'), "OK",
        ok => {
          this.refreshFormValue();
        });
    }
  }

  onback(e) {
    e.preventDefault();
    this._location.back();
  }

  getDepartmentById(id) {
    const self = this;

    this.networkSevice.get(this._baseUrl + "api/Department/" + id, null, function (res) {
      self.department['id'] = res['id'];
      self.department['name'] = res['name'];
      self.department['parentId'] = res['parentId'];
      self.department['leaderStaffId'] = res['leaderStaffId'];
    });
  }

  getAllDepartment() {
    const self = this;

    this.networkSevice.get(this._baseUrl + "api/Department", null, function (res) {
      self.lstDepartment = res;
    });
  }
  getAllStaff() {
    const self = this;

    this.networkSevice.get(this._baseUrl + "api/Staff", null, function (res) {
      self.lstStaff = res;
    });
  }
}
