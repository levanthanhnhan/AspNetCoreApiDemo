import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';

@Component({
  selector: 'app-staff-detail',
  templateUrl: './staff-detail.component.html',
  styleUrls: ['./staff-detail.component.scss']
})

export class StaffDetailComponent implements OnInit {
  staff: any;

  constructor(
    private _location: Location,
    private _http: HttpClient,
    @Inject('BASE_URL') private _baseUrl: string,
    private _route: ActivatedRoute) { }

  ngOnInit(): void {
    this._route.paramMap.subscribe((params: ParamMap) => {
      this.getStaffById(params.get('id'))
    });
  }

  getStaffById(id) {
    return this._http.get(this._baseUrl + "api/Staff/" + id).subscribe(res => {
      this.staff = res
    })
  }

  back() {
    this._location.back();
  }
}
