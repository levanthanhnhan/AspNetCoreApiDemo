import { Component, OnInit, Inject, ViewChild, ElementRef, EventEmitter, Output, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NetworkingService } from '../../common/services/networking.service';
import { MatDialog } from '@angular/material/dialog';

interface DeviceType {
  id: number;
  name: string;
  code: string;
  note: string;
}

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})

export class SearchComponent implements OnInit {
  selectedDep: any = null;
  selectedPos: any = null;
  listDept: any;
  listPos: any;
  deptActive: number;
  posActive: number;
  deviceTypeActive: number;
  searchList: any = [];
  satffIdList: any;

  @Output() onSearch = new EventEmitter()
  @Output() onClear = new EventEmitter()
  @Input() isShowStaffContainer = false;
  @ViewChild('txtSearch') txtSearch: ElementRef;
  @ViewChild('txtStartDate') txtStartDate: ElementRef;
  @ViewChild('txtEndDate') txtEndDate: ElementRef;

  constructor(private translate: TranslateService, private networkSevice: NetworkingService, @Inject('BASE_URL') private baseUrl: string, public dialog: MatDialog) {
    // constructor
  }

  ngOnInit(): void {
    const self = this;

    // Get data
    this.getData(function () {
      self.selectedDep = 0;
      self.selectedPos = 0;
    });
  }

  getData(next?) {
    const self = this;
    const requests = [
      this.networkSevice.makeObjectRequest(this.baseUrl + "api/Department"),
      this.networkSevice.makeObjectRequest(this.baseUrl + "api/Position")
    ]

    this.networkSevice.multipleRequests(requests,
      res => {
        self.listDept = res[0];
        self.listPos = res[1];
        next();
      },
      err => {
        console.log(err);
      }
    );
  }

  onChangeDepartment(event) {
    this.deptActive = parseInt(event.target.value);
  }

  onChangePosition(event) {
    this.posActive = parseInt(event.target.value);
  }

  search() {
    const params = {
      DepartmentId: this.deptActive,
      PositionId: this.posActive,
      StaffName: this.txtSearch.nativeElement.value.replace(/ +(?= )/g, '').trim(),
      DeviceName: $('#search').val(),
      StartDate: $('#txtStartDate').val(),
      EndDate: $('#txtEndDate').val()
    }
    this.onSearch.emit(params);
    this.txtSearch.nativeElement.value = this.txtSearch.nativeElement.value.replace(/ +(?= )/g, '').trim();
  }

  clear() {
    this.deptActive = 0;
    this.posActive = 0;
    this.deviceTypeActive = 0;
    this.txtSearch.nativeElement.value = '';
    this.txtStartDate.nativeElement.value = '';
    this.txtEndDate.nativeElement.value = '';

    const params = {
      DepartmentId: 0,
      PositionId: 0,
      StaffName: "",
    }

    this.onClear.emit(params);
  }

}
