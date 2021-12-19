import { Component, OnInit, Inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NetworkingService } from '../../../../common/services/networking.service';
import { ModalService } from '../../../../common/services/modal.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Department } from '../../models/department-model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-department-regist',
  templateUrl: './department-regist.component.html',
  styleUrls: ['./department-regist.component.scss']
})

export class DepartmentRegistComponent implements OnInit {
  addForm: FormGroup;
  lstDepartment: any;
  lstStaff: any;
  submitted = false;
  accountStorage: any;

  constructor(
    private _location: Location,
    private translate: TranslateService,
    private formBuilder: FormBuilder,
    private networkSevice: NetworkingService,
    @Inject('BASE_URL') private _baseUrl: string,
    private modalService: ModalService) {
    this.accountStorage = this.networkSevice.getAccount();
  }
  ngOnInit(): void {
    this.getAllDepartment();
    this.getAllStaff();
    this.addForm = this.formBuilder.group({
      departmentName: ['', Validators.required],
      leaderDepartment: [0, Validators.required],
      parentDepartmentId: [0, Validators.required]
    });
  }

  get regisData() { return this.addForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.addForm.invalid) {
      return;
    }
    let department = new Department();
    department.parentId = +this.addForm.get("parentDepartmentId").value;
    department.name = this.addForm.get("departmentName").value.toUpperCase();
    department.leaderStaffId = +this.addForm.get("leaderDepartment").value;
    department.createId = this.accountStorage.userId;
    department.updateId = this.accountStorage.userId;
    this.networkSevice.post<any>(this._baseUrl + "api/Department/Create", JSON.stringify(department), null,
      res => {
        if (res._statusCode == 1) {
          this.modalService.openModalSuccess(this.translate.instant('Modal.Title.Success'), this.translate.instant('DepartmentAdd.AddSuccess'), "OK",
            ok => {
              this._location.back();
          });
        } else {
          this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('DepartmentAdd.AddFail'), "OK");
        } 
      }
      ,
      err => {
        this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('DepartmentAdd.AddFail'),"OK");
      }
    );
  }

  onback(e) {
    e.preventDefault();
    this._location.back();
  }

  async getAllDepartment() {
    const self = this;

    await this.networkSevice.get(this._baseUrl + "api/Department", null, function (res) {
      self.lstDepartment = res;
    });
  }

  async getAllStaff() {
    const self = this;

    await this.networkSevice.get(this._baseUrl + "api/Staff", null, function (res) {
      self.lstStaff = res;
    });
  }
}
