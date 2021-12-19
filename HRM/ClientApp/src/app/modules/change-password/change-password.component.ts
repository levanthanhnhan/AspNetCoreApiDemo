import { Component, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NetworkingService } from '../../common/services/networking.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComparePassword } from '../account/services/customvalidator.validator';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})

export class ChangePasswordComponent implements OnInit {
  submitted = false;
  changepasswordFormGroup: FormGroup;
  isShowChangePasswordError = false;
  isShowChangePasswordSuccess = false;
  isShowLoginAgain = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private service: NetworkingService,
    private modal: NgbActiveModal,
    @Inject('BASE_URL') private _baseUrl: string, ) { }

  get changepasswordData() { return this.changepasswordFormGroup.controls; }

  ngOnInit() {
    this.changepasswordFormGroup = this.formBuilder.group(
      {
        passwordOld: ['', [Validators.required]],
        passwordNew: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required]
      },
      {
        // Used custom form validator name
        validator: ComparePassword("passwordNew", "confirmPassword")
      }
    );
  }

  onSubmit() {
    this.submitted = true;

    if (this.changepasswordFormGroup.invalid) {
      return;
    }
    
    this.changePassword();
  }

  hideModal() {
    this.modal.close();
  }
 
  changePassword() {
    var passwordOld = this.changepasswordFormGroup.value.passwordOld;
    var passwordNew = this.changepasswordFormGroup.value.passwordNew;
    var userName = this.service.getAccount().userName;

    var model = {
      UserName: userName,
      Password: passwordNew,
      OldPassword: passwordOld
    };

    const self = this;
    this.service.post(this._baseUrl + "api/ChangePassword", model, null, function (res) {
      if (res['_msg'] == "Old password is incorrect!") {
        self.isShowChangePasswordError = true;
        self.isShowChangePasswordSuccess = false;
        self.isShowLoginAgain = false;
      }
      else {
        self.isShowChangePasswordSuccess = true;
        self.isShowChangePasswordError = false;
        self.isShowLoginAgain = true;
      }
    });
  }
    
  backToLogin() {
    this.router.navigateByUrl('/Login');
  }
}
