import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { NetworkingService } from '../../common/services/networking.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from '../../modules/change-password/change-password.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NavigationModel } from './navigation-model';

@Component({
  selector: 'app-nav',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})

export class NavigationComponent {
  account: any;
  listAccessByRole: NavigationModel[] = [];

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' })
  };

  constructor(private router: Router,
              private networking: NetworkingService,
              private http: HttpClient,
              private modal: NgbModal,
              @Inject('BASE_URL') private baseUrl: string)
  {
    this.account = this.networking.getAccount();
  }

  ngOnInit() {
    const self = this;

    this.networking.post(this.baseUrl + "api/Role/GetAccesssByRoleId", this.account.roleId, null, function (res) {
      self.listAccessByRole = res;

      if (self.listAccessByRole.length == 0) {
        let displayEmptyNav = new NavigationModel();
        displayEmptyNav.name = '/Dashboard';
        displayEmptyNav.routerLink = '/Dashboard';
        displayEmptyNav.nameTrans = 'Header.Dashboard';

        self.listAccessByRole.push(displayEmptyNav);
      }
    });
  }

  showProfile() {
    this.router.navigateByUrl("Staff/" + this.account.userId)
  }

  logout() {
    const self = this;

    this.http.post<any>(this.baseUrl + "api/Logout", JSON.stringify(this.account.userName), this.httpOptions).subscribe(res => {
      self.networking.deleteCookies();
      self.router.navigateByUrl("Login");
    });
  }

  changePassword() {
    this.modal.open(ChangePasswordComponent, {
      keyboard: false,
      backdrop: 'static'
    });
  }
}
