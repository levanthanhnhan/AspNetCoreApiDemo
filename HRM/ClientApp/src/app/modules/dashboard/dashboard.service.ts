import { Injectable, Inject } from '@angular/core';
import { NetworkingService } from '../../common/services/networking.service';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class DashboardService {
  private subject = new Subject<any>();

  constructor(private networkSevice: NetworkingService, @Inject('BASE_URL') private baseUrl: string) { }

  setData(): any {

    const requests = [
      this.networkSevice.makeObjectRequest(this.baseUrl + "api/Dashboard/GetStaffByYear"),
      this.networkSevice.makeObjectRequest(this.baseUrl + "api/Dashboard/GetDepartments")
    ]

    this.networkSevice.multipleRequests(requests,
      res => {
        this.subject.next(res);
      },
      err => {
        console.log(err);
      }
    )
  }

  getData(): Observable<any> {
    return this.subject.asObservable();
  }
}
