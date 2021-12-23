import { Component, Inject, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NetworkingService } from '../../common/services/networking.service';
import { ModalService } from '../../common/services/modal.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

enum CellType {
  CheckBoxUncheck = 0,
  CheckBoxCheck = 1,
  TextBox = 2
}

interface Access {
  id: number,
  name: string
}

interface Role {
  id: number,
  name: string,
  isInsert: boolean
}

interface RoleAccess {
  accessId: number,
  roleId: number,
}

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class RoleComponent implements OnInit {

  public listRole: Role[];
  public listAccess: Access[];
  public listRoleAccess: RoleAccess[];
  public listRoleDelete: Role[] = [];
  result: any;
  public maxID: number = 0;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private translate: TranslateService,
    private networkSevice: NetworkingService,
    private modalService: ModalService) {
  }

  ngOnInit() {
    const self = this;

    const requests = [
      this.networkSevice.makeObjectRequest(this.baseUrl + "api/Access"),
      this.networkSevice.makeObjectRequest(this.baseUrl + "api/Role"),
      this.networkSevice.makeObjectRequest(this.baseUrl + "api/Role/GetRoleAccesss")
    ]

    this.networkSevice.multipleRequests(requests,
      res => {
        self.listAccess = res[0];
        self.listRole = res[1];
        self.listRoleAccess = res[2];
        self.buildTable($("#table"));
      },
      err => {
        console.log(err);
      }
    )
  }

  buildTable(table) {
    var columns = [];
    var data = [];
    var self = this;

    columns.push(
      {
        field: 'state0',
        checkbox: true,
        valign: 'middle'
      },
      {
        field: 'state1',
        checkbox: false,
        valign: 'middle',
        formatter: function (arr) {
          return '<input type="text" class="role-name" value="' + arr[1] + '" name="' + arr[0] + '"/>';
        },
      },
    );

    for (var i = 0; i < this.listAccess.length; i++) {
      columns.push({
        field: 'field' + i,
        title: this.listAccess[i].name,
        sortable: false,
        valign: 'middle',
        align: 'center',
        formatter: function (arr) {
          if (arr[0]) {
            return '<input type="checkbox" checked="checked" class="cell-check" id="' + arr[1] + '"/>'
          }
          else {
            return '<input type="checkbox" class="cell-check" id="' + arr[1] + '"/>'
          }
        }
      })
    }

    for (var i = 0; i < this.listRole.length; i++) {
      var row = {}
      row['state1'] = [this.listRole[i].id, this.listRole[i].name];

      for (var j = 0; j < this.listAccess.length; j++) {
        const idx = this.listRoleAccess.findIndex(roleAccess => roleAccess.accessId == this.listAccess[j].id && roleAccess.roleId == this.listRole[i].id);
        if (idx != -1) {
          row['field' + j] = [true, (i + "-" + j)];
        }
        else {
          row['field' + j] = [false, (i + "-" + j)];
        }
      }
      data.push(row);
    }

    table.bootstrapTable('destroy').bootstrapTable({
      columns: columns,
      data: data,
      toolbar: '.toolbar',
      search: false,
      showColumns: false,
      clickToSelect: false,
      fixedColumns: true,
      fixedNumber: 2,

      // Check in checkbox role
      onCheck: function (row, $element) {
        $('#remove').prop('disabled', false);
        var rowIdx = $element[0].dataset.index;
        self.listRoleDelete.push({
          id: self.listRole[rowIdx].id,
          name: self.listRole[rowIdx].name,
          isInsert: true
        });
        self.listRoleDelete.length == self.listRole.length ? $('#remove').prop('disabled', true) : $('#remove').prop('disabled', false);
      },

      // Uncheck in checkbox role
      onUncheck: function (row, $element) {
        var rowIdx = $element[0].dataset.index;
        var index = self.listRoleDelete.findIndex(role => role.id == self.listRole[rowIdx].id);

        if (index > -1) {
          self.listRoleDelete.splice(index, 1);
        }
        self.listRoleDelete.length == 0 ? $('#remove').prop('disabled', true) : $('#remove').prop('disabled', false);
      }
    });

    $("input[name='btSelectAll']").remove();

    $(".role-name").on('input', function () {
      var roleId = $(this)[0]["name"];
      var roleName = $(this)[0]["value"];

      // Check exist name
      if ((self.listRole.find(role => role.name.toUpperCase() == roleName.toUpperCase()) != undefined)) {
        $(this).addClass("role-name-error");
      } else {
        $(this).removeClass("role-name-error");
      }

      self.listRole.find(role => role.id == roleId).name = roleName;
    });
  }

  saveChanges() {
    for (var i = 0; i < this.listRole.length; i++) {
      for (var j = 0; j < i; j++) {
        if (this.listRole[i].name.toUpperCase().replace(/ +(?= )/g, '').trim() == this.listRole[j].name.toUpperCase().replace(/ +(?= )/g, '').trim()) {
          this.modalService.openModalDelete(this.translate.instant('Modal.Title.Error'), this.translate.instant('Role.Message.Error'),"[" + this.listRole[i].name + "] ", this.translate.instant('Modal.Action.Ok'), Ok => { });
          return;
        }
        if (this.listRole[i].name == "") {
          this.modalService.openModalError(this.translate.instant('Modal.Title.Error'), this.translate.instant('Role.Message.RolenameRequired'), this.translate.instant('Modal.Action.Ok'), Ok => { });
          $("input[name='" + this.listRole[i].id + "']").focus();
          $("input[name='" + this.listRole[i].id + "']").addClass("role-name-error");
          return;
        }
      }
    }
    let listSaveChanges: RoleAccess[] = [];

    // Loop all row, col grid view
    for (var i = 0; i < this.listRole.length; i++) {
      for (var j = 0; j < this.listAccess.length; j++) {

        // Get value by row-col
        var checked = $("#" + i + "-" + j).is(':checked');

        if (checked) {
          // Create object RoleAccess
          var obj: RoleAccess = {
            roleId: this.listRole[i].id,
            accessId: this.listAccess[j].id
          }

          // Add value is checked
          listSaveChanges.push(obj);
        }
      }
    }

    // Create body data using post request
    var bodyData = {
      ListRoleDelete: this.listRoleDelete,
      ListRoleInsertUpdate: this.listRole,
      ListRoleAccess: listSaveChanges,
    }

    // Call API
    const self = this;
    this.networkSevice.post(this.baseUrl + "api/Role/SaveChanges", bodyData, null,
      res => {
        self.ngOnInit();
        self.modalService.openModalError(this.translate.instant('Modal.Title.Success'), this.translate.instant('Role.Message.SaveChange'), this.translate.instant('Modal.Action.Ok'), Ok => { });
        return;
      },
      (error: HttpErrorResponse) => {
        console.log(error.message);
      });
  }

  addNewRole() {
    $('#remove').prop('disabled', true)
    this.listRole.forEach(role => {
      if (role.id > this.maxID) {
        this.maxID = role.id;
      }
    });

    this.listRole.push({
      isInsert: true,
      id: this.maxID + 1,
      name: ""
    });
    this.buildTable($("#table"));

    $("input[name='" + (this.maxID + 1) + "']").focus();
  }

  deleteClick() {
    const self = this;
    $('#remove').prop('disabled', true)
    this.modalService.openModalConfirm(this.translate.instant('Modal.Title.Confirm'), this.translate.instant('Modal.Message.ConfirmDel'),
      Ok => {
        // get indexRoleDel at listRole => remove indexRoleDel in listRole
        for (var i = 0; i < this.listRoleDelete.length; i++) {
          var indexRoleDel = this.listRole.findIndex(role => role.id == this.listRoleDelete[i].id);
          if (indexRoleDel > -1) {
            this.listRole.splice(indexRoleDel, 1);
          }
        }

        // build Table
        this.buildTable($("#table"));
      },
      Cancel => {
      })
  }
}
