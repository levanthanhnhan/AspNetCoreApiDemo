import { Component, OnInit, Inject, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Location } from '@angular/common';
import { NetworkingService } from '../../../../common/services/networking.service';
import { Role } from '../../../../common/constants/constants';

@Component({
  selector: 'app-staff-information',
  templateUrl: './staff-information.component.html',
  styleUrls: ['./staff-information.component.scss']
})

export class StaffInformationComponent implements OnInit, AfterViewInit {
  staff: any;
  isAdminRole: boolean;
  loginId: any;
  staffId: string;
  isShowInfomation: boolean;

  constructor(
    private _location: Location,
    private networking: NetworkingService,
    @Inject('BASE_URL') private _baseUrl: string,
    private _route: ActivatedRoute,
    private _routing: Router) {

  }

  ngAfterViewInit() {
    $("#tableInfomation").css("min-height", $("#leftContainer").height());
    $("#tableSkill").css("min-height", $("#leftContainer").height());
  }

  ngOnInit(): void {
    this.staff = {};
    this._route.paramMap.subscribe((params: ParamMap) => {
      this.staffId = params.get('id');
      this.getUserById(params.get('id'))
    });
    this.loginId = this.networking.getAccount().userId.toString();
    this.isShowInfomation = true;
  }

  getUserById(id) {
    const self = this;
    this.networking.get(this._baseUrl + "api/Staff/StaffInfoById/" + id, null, function (res) {
      self.staff = res;
    });
  }

  handleItemSelect(id) {
    $(".list-group-item").removeClass("item-active");
    $("#" + id).addClass("item-active");
    this.isShowInfomation = id == "itemInfo" ? true : false;
  }

  editProfile(id) {
    let url = '/Staff/' + id + '/Edit';
    this._routing.navigateByUrl(url);
  }
}
