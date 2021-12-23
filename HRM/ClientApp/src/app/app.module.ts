import { BrowserModule, HammerModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CookieService } from 'ngx-cookie-service';
import { DeferLoadModule } from '@trademe/ng-defer-load';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import { NgSelectModule } from '@ng-select/ng-select';

import { AppComponent } from './app.component';
import { LoginComponent } from './modules/login/pages/login.component';
import { HomeComponent } from './modules/home/home.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { DashboardComponent } from './modules/dashboard/pages/dashboard.component';
import { StaffChartComponent } from './modules/dashboard/components/staff-chart/staff-chart.component';
import { StaffComponent } from './modules/staff/pages/staff/staff.component';
import { ModalErrorComponent } from './common/modals/error/modal-error.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { LoginService } from './modules/login/services/login.service';
import { DatePipe } from '@angular/common';
import { ExcelService } from './modules/staff/services/excel.service';
import { NavigationComponent } from './header/navigation/navigation.component';
import { LanguageComponent } from './header/language/language.component';
import { DepartmentChartComponent } from './modules/dashboard/components/department-chart/department-chart.component';
import { ProjectChartComponent } from './modules/dashboard/components/project-chart/project-chart.component';
import { AccountRegisterComponent } from './modules/account/pages/register/account-register.component';
import { ModalSuccessComponent } from './common/modals/success/modal-success.component';
import { StaffEditComponent } from './modules/staff/pages/staff-edit/staff-edit.component';
import { StaffInformationComponent } from './modules/staff/pages/staff_infomation/staff-information.component';
import { StaffRegisterComponent } from './modules/staff/pages/staff-register/staff-register.component';
import { NetworkingService } from './common/services/networking.service';
import { ModalConfirmComponent } from './common/modals/confirm/modal-confirm.component';
import { ModalService } from './common/services/modal.service';
import { ChangePasswordComponent } from './modules/change-password/change-password.component';
import { DashboardService } from './modules/dashboard/dashboard.service';
import { LoadingComponent } from './modules/loading/loading.component';
import { StaffListComponent } from './modules/staff/pages/staff-list/staff-list.component';
import { SearchComponent } from './modules/search/search.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DepartmentComponent } from './modules/department/pages/department/department.component';
import { DepartmentEditComponent } from './modules/department/pages/department-edit/department-edit.component';
import { DepartmentRegistComponent } from './modules/department/pages/department-regist/department-regist.component';
import { DepartmentListComponent } from './modules/department/pages/department-list/department-list.component';
import * as $ from 'jquery';
import { AccountComponent } from './modules/account/pages/account/account.component';
import { AccountListComponent } from './modules/account/pages/list/account-list.component';
import { AccountEditComponent } from './modules/account/pages/edit/account-edit.component';
import { SignUpComponent } from './modules/signup/signup.component';
import { RoleComponent } from './modules/role/role.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  imports: [
    HammerModule,
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ChartsModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    DeferLoadModule,
    NgxPaginationModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.circle,
      backdropBackgroundColour: 'rgba(0,0,0,0.4)',
      backdropBorderRadius: '4px',
      primaryColour: '#F1319C',
      secondaryColour: '#1F1963',
      tertiaryColour: '#ffffff'
    }),
    BrowserAnimationsModule,
    NgSelectModule
  ],

  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    HeaderComponent,
    FooterComponent,
    DashboardComponent,
    StaffChartComponent,
    StaffComponent,
    StaffEditComponent,
    ModalErrorComponent,
    NavigationComponent,
    LanguageComponent,
    DepartmentChartComponent,
    ProjectChartComponent,
    AccountComponent,
    AccountListComponent,
    AccountEditComponent,
    AccountRegisterComponent,
    ModalSuccessComponent,
    StaffInformationComponent,
    StaffRegisterComponent,
    LoadingComponent,
    ChangePasswordComponent,
    StaffListComponent,
    SearchComponent,
    DepartmentComponent,
    DepartmentEditComponent,
    DepartmentRegistComponent,
    DepartmentListComponent,
    SignUpComponent,
    RoleComponent
  ],
  entryComponents: [
    ModalErrorComponent,
    ModalSuccessComponent,
    ModalConfirmComponent,
  ],

  providers: [
    CookieService,
    LoginService,
    DatePipe,
    ExcelService,
    NetworkingService,
    ModalService,
    DashboardService,
  ],

  bootstrap: [AppComponent]
})
export class AppModule { }
