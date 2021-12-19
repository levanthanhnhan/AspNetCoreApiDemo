import { Component, OnInit, Inject, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';
import { StaffService } from '../../services/staff.service';
import { NetworkingService } from '../../../../common/services/networking.service';
import { Location } from '@angular/common';
import { CookieService } from 'ngx-cookie-service';

@Component({
    selector: 'app-staff-edit',
    templateUrl: './staff-edit.component.html',
    styleUrls: ['./staff-edit.component.scss']
})

export class StaffEditComponent implements OnInit {
    staff : any;
    positionList: any;
    departmentList: any;
    // error
    error = {
        dateOfBirth: {
            status: false,
        },
        sex: {
            status: false,
        },
        phone: {
            status: false,
        },
        contractDate: {
            status: false,
        },
        email: {
            status: false,
        },
        departmentId: {
            status: false,
        },
        positionId: {
            status: false,
        },
    };
    const = {
        INT_DEFAULT_VALUE: -1,
        DATETIME_DEFAULT_VALUE: "1900-01-01",
        DATETIME_FORMAT: "yyyy-MM-dd",
        EMAIL_PATTERN: /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/gi,
    }

    httpOptions = {
        headers: new HttpHeaders(
            {
                'Authorization': this.getAccessToken(),
            }
        )
    };

    getAccessToken(): string {
        return (this.cookie.get("token") === "") ? null : "Bearer " + JSON.parse(this.cookie.get("token")).accessToken;
    }

    constructor(
        private staffService: StaffService,
        private _location: Location,
        private cookie: CookieService,
        private netService: NetworkingService,
        @Inject('BASE_URL') private _baseUrl: string,
        private _route: ActivatedRoute,
        
    ) {}

    //init
    ngOnInit(): void {
        this.staff = {};
        this._route.paramMap.subscribe(async (params: ParamMap) => {
            await this.getStaffById(params.get('id'));
        });
        this.getPositionList();
        this.getDepartmentList();
    }

    //get Staff By Id
    async getStaffById(id) {
        let self = this;
        return this.netService.get(this._baseUrl + "api/Staff/" + id, null, function(res){
            self.staff = res;
            self.staff["dateOfBirth"] = res["dateOfBirth"].substr(0, 10);
            self.staff["contractDate"] = res["contractDate"].substr(0, 10);
        });
    };

    // get Position List
    getPositionList() {
        let self = this;
        return this.netService.get(this._baseUrl + "api/Position", null, function (res) {
            self.positionList = res;
        });
    }

    // get Department List
    getDepartmentList() {
        let self = this;
        return this.netService.get(this._baseUrl + "api/Department", null, function (res) {
            self.departmentList = res;
        });
    }

    // onChange form
    onChange() {
        return this.validate(this.staff);
    }

    //submit form
    onSubmit() {
      if (this.validate(this.staff)) {
        this.staffService.update(this.staff);
      }
    }

    //reset error
    resetError() {
        this.error.dateOfBirth.status = false;
        this.error.sex.status = false;
        this.error.phone.status = false;
        this.error.email.status = false;
        this.error.contractDate.status = false;
        this.error.departmentId.status = false;
        this.error.positionId.status = false;

    }

    // convert request
    convertRequest() {
        this.staff['departmentId'] = this.staff['departmentId'] == '' ? this.const.INT_DEFAULT_VALUE : this.staff['departmentId'];
        this.staff['positionId'] = this.staff['positionId'] == '' ? this.const.INT_DEFAULT_VALUE : this.staff['positionId'];
    }

    //validate
    validate(staff) {
        //convert request
        this.convertRequest();
        // clear error
        this.resetError();
        let flag = true;
        //dateOfBirth
        if (!this.checkDate(this.staff['dateOfBirth'])) {
            this.error.dateOfBirth.status = true;
            flag = false;
        }
        //sex
        if (Number.isNaN(Number(staff['sex']))) {
            this.error.sex.status = true;
            flag = false;
        }
        //phone
        if (!this.isNumberInteger(staff['phone'])) {
            this.error.phone.status = true;
            flag = false;
        }
        //contractDate
        if (!this.checkDate(this.staff['contractDate'])) {
            this.error.contractDate.status = true;
            flag = false;
        }
        //departmentId
        if (!this.isSelected(staff['departmentId'])) {
            flag = this.error.departmentId.status = true;
            flag = false;
        }
        //positionId
        if (!this.isSelected(staff['positionId'])) {
            flag = this.error.positionId.status = true;
            flag = false;
        }
        //email
        if (!this.checkEmail(staff['email'])) {
            flag = this.error.email.status = true;
            flag = false;
        }
        //return
        return flag;
    }

    // number int check
    isNumberInteger(n) {
        if (Number.isNaN(Number(n))) {
            return false;
        }
        if (Number(n) >= -1 && Number(n) % 1 === 0) {
            return true;
        }
        return false;
    }

    // check dropdown
    isSelected(n) {
        if (Number.isNaN(Number(n))) {
            return false;
        }
        if (Number(n) > -1 && Number(n) % 1 === 0) {
            return true;
        }
        return false;
    }

    // check date
    checkDate(date) {
        if (date == '') {
            return true;
        }
        let $date = new Date(date);

        return ($date < new Date() && $date.getFullYear() >=1753)
    }

    //check email
    checkEmail(email: string) {
        if (email != '' && !email.match(this.const.EMAIL_PATTERN)) {
            return false;
        }
        return true;
    }

    //onFileChanged
    previewUrl: any;
    fileData: File = null;
    onFileChanged(event) {
        this.fileData = event.target.files[0]
        this.preview();
    }

    //preview
    preview() {
        // Show preview 
        var mimeType = this.fileData.type;
        if (mimeType.match(/image\/*/) == null) {
            return;
        }

        var reader = new FileReader();
        reader.readAsDataURL(this.fileData);
        reader.onload = (_event) => {
            this.previewUrl = reader.result;
        }
    }

    //cancle button
    cancle(e) {
        e.preventDefault();
        this._location.back();
    }
    // press enter
    noSubmit(e) {
        e.preventDefault();
        return;
    }
}
