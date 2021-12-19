import { Component, Input, OnInit } from '@angular/core'
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-modal-confirm',
  templateUrl: './modal-confirm.component.html',
  styleUrls: ['./modal-confirm.component.css']
})

export class ModalConfirmComponent implements OnInit {

  @Input() modalTitle: string = '';
  @Input() modalContent: string = '';
  public titleOk = null;
  public titleCancel = null;

  constructor(public activeModal: NgbActiveModal, private translate: TranslateService) {
  }

  ngOnInit() {
    this.titleOk = this.translate.instant("ModalConfirm.Ok");
    this.titleCancel = this.translate.instant("ModalConfirm.Cancel");
  }

  cancel() {
    this.activeModal.close(false);
  }

  ok() {
    this.activeModal.close(true);
  }
}
