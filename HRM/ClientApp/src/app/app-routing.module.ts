import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './modules/login/pages/login.component';
import { HomeComponent } from './modules/home/home.component';
import { DashboardComponent } from './modules/dashboard/pages/dashboard.component';
import { StaffComponent } from './modules/staff/pages/staff/staff.component';
import { AccountRegisterComponent } from './modules/account/pages/register/account-register.component';
import { StaffEditComponent } from './modules/staff/pages/staff-edit/staff-edit.component';
import { StaffInformationComponent } from './modules/staff/pages/staff_infomation/staff-information.component';
import { StaffRegisterComponent } from './modules/staff/pages/staff-register/staff-register.component';
import { StaffListComponent } from './modules/staff/pages/staff-list/staff-list.component';
import { DepartmentComponent } from './modules/department/pages/department/department.component';
import { DepartmentEditComponent } from './modules/department/pages/department-edit/department-edit.component';
import { DepartmentRegistComponent } from './modules/department/pages/department-regist/department-regist.component';
import { DepartmentListComponent } from './modules/department/pages/department-list/department-list.component';
import { AccountListComponent } from './modules/account/pages/list/account-list.component';
import { AccountComponent } from './modules/account/pages/account/account.component';
import { AccountEditComponent } from './modules/account/pages/edit/account-edit.component';
import { SignUpComponent } from './modules/signup/signup.component';
import { RoleComponent } from './modules/role/role.component';


const routes: Routes = [
  {
    path: 'Login',
    component: LoginComponent,
  },
  {
    path: 'SignUp',
    component: SignUpComponent,
  },
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: 'Dashboard',
        component: DashboardComponent,
      },
      {
        path: 'Staff',
        component: StaffComponent,
        children: [
          {
            path: 'List',
            component: StaffListComponent,
          },
          {
            path: ':id/Detail',
            component: StaffInformationComponent,
          },
          {
            path: ':id/Edit',
            component: StaffEditComponent,
          },
          {
            path: 'Register',
            component: StaffRegisterComponent,
          },
        ]
      },
      {
        path: 'Account',
        component: AccountComponent,
        children: [
          {
            path: 'List',
            component: AccountListComponent,
          },
          {
            path: ':id/Edit',
            component: AccountEditComponent,
          },
          {
            path: 'Register',
            component: AccountRegisterComponent,
          }
        ]
      },
      {
        path: 'Department',
        component: DepartmentComponent,
        children: [
          {
            path: 'List',
            component: DepartmentListComponent,
          },
          {
            path: ':id/Edit',
            component: DepartmentEditComponent,
          },
          {
            path: 'Regist',
            component: DepartmentRegistComponent,
          }
        ]
      },
      {
        path: 'Role',
        component: RoleComponent,
      }
    ]
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],

  declarations: [
  ],

  exports: [RouterModule]
})
export class AppRoutingModule { }
