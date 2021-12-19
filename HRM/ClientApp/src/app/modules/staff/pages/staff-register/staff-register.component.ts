
import { Component, OnInit, Inject, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, ParamMap, RouterLink } from '@angular/router';

import { FormBuilder, FormGroup, Validators, AbstractControl} from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { StaffModel } from '../../models/staff-model';
import { NetworkingService } from '../../../../common/services/networking.service';
import { TranslateService } from '@ngx-translate/core';
import { ModalService } from '../../../../common/services/modal.service';
import { CheckDate } from '../../services/CheckDate';
import { StaffService } from '../../services/staff.service';



@Component({
  selector: 'app-staff-register',
  templateUrl: './staff-register.component.html',
  styleUrls: ['./staff-register.component.scss']
})

export class StaffRegisterComponent implements OnInit {

  submitted = false;

  langActive = "en";

  httpOptions = {
    headers: this.networkSevice.httpOptions.headers.delete('Content-Type')
  };

  msg: any;

  userId: string;
  regFormGroup: FormGroup;
  myForm: FormGroup;
  isShowLoginError = false;
  position: any;
  deparment: any;
  imgLink: any;
  message: boolean;


  constructor(
    private _http: HttpClient,
    @Inject('BASE_URL') private _baseUrl: string,
    private _route: ActivatedRoute,
    private cookie: CookieService,
    private formBuilder: FormBuilder,
    private networkSevice: NetworkingService,
    private translate: TranslateService,
    private modalService: ModalService,
    private staffService: StaffService,
    
  ) {
    const lang = this.cookie.get("lang") === "" ? "en" : this.cookie.get("lang");
    this.langActive = lang;
    this.translate.setDefaultLang(lang);
    this.translate.use(lang);
  }

  ngOnInit() {
    //get cookie
    var data = this.cookie.get('account');
    const userStorage = JSON.parse(data);
    this.userId = userStorage.userId;
   
    this.regFormGroup = this.formBuilder.group({
      firstName: [null, [Validators.required]],
      lastName: [null, Validators.required],
      sex: [null, Validators.required],
      dateOfBirth: [null, [Validators.required, CheckDate]],
      address: [null],
      phone: [null, [Validators.pattern("^[0-9 \\+\\-\\(\\)]*$")]],
      email: [null, [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]],
      contractDate: [null, CheckDate],
      departmentId: [null],
      positionId: [null],
    });

    const self = this;

    this.networkSevice.get(this._baseUrl + "api/Position", null, function (res) {
      self.position = res;
    });
    this.networkSevice.get(this._baseUrl + "api/Department", null, function (res) {
      self.deparment = res;
    });
    
  }
  get regData() { return this.regFormGroup.controls; }

  onSubmit() {
    let formData: FormData = new FormData();

    this.submitted = true;
    this.message = false;

    if (this.regFormGroup.invalid) {
      return;
    }

    let staffModel: StaffModel = this.regFormGroup.value;

    // If departmentId is either null or empty, setting default value
    if (this.regFormGroup.value.departmentId == null || this.regFormGroup.value.departmentId == "") {
      this.regFormGroup.value.departmentId = this.const.INT_DEFAULT_VALUE;
    }
    staffModel.departmentId = parseInt(this.regFormGroup.value.departmentId, 10);

    // If positionId is either null or empty, setting default value
    if (this.regFormGroup.value.positionId == null || this.regFormGroup.value.positionId == "") {
      this.regFormGroup.value.positionId = this.const.INT_DEFAULT_VALUE;
    }
    staffModel.positionId = parseInt(this.regFormGroup.value.positionId, 10);

    // If contractDate is either null or empty, setting default value
    if (this.regFormGroup.value.contractDate == null || this.regFormGroup.value.contractDate == "") {
      this.regFormGroup.value.contractDate = new Date(this.const.DATETIME_DEFAULT_VALUE);
    }

    // Register
    this.staffService.register(staffModel);
  }
 
  back(e) {
    e.preventDefault();
  }

  const = {
    INT_DEFAULT_VALUE: "-1",
    DATETIME_DEFAULT_VALUE: "1900-01-01"
  }

  noSubmit(e) {
    e.preventDefault();
  }
}
