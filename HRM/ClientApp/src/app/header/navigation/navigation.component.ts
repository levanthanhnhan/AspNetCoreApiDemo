import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { NetworkingService } from '../../common/services/networking.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangePasswordComponent } from '../../modules/change-password/change-password.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Role } from '../../common/constants/constants';

@Component({
  selector: 'app-nav',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})

export class NavigationComponent {
  account: any;

  displayNav = {
    Dashboard: false,
    Staff: false,
    Department: false,
    Account: false
  }

  constructor(private router: Router,
              private networking: NetworkingService,
              private http: HttpClient,
              private modal: NgbModal,
              @Inject('BASE_URL') private baseUrl: string)
  {
    this.account = this.networking.getAccount();
    this.isVisibility(this.account);
  }

  showProfile() {
    this.router.navigateByUrl("Staff/" + this.account.userId)
  }

  logout() {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' })
    };

    const self = this;

    this.http.post<any>(this.baseUrl + "api/Logout", JSON.stringify(this.account.userName), httpOptions).subscribe(res => {
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

  isVisibility(account: any) {
    let roleId = account.roleId;

    switch (roleId) {
      case Role.GA: {
        this.displayNav = {
          Dashboard: true,
          Staff: true,
          Department: false,
          Account: false
        }
        break;
      }

      case Role.Manager: {
        this.displayNav = {
          Dashboard: true,
          Staff: true,
          Department: true,
          Account: true
        }
        break;
      }

      case Role.Leader: {
        this.displayNav = {
          Dashboard: true,
          Staff: true,
          Department: false,
          Account: false
        }
        break;
      }

      case Role.Member: {
        this.displayNav = {
          Dashboard: true,
          Staff: false,
          Department: false,
          Account: false
        }
        break;
      }
    } 
  }
}
