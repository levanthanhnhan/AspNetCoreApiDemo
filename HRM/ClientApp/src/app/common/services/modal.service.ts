import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalConfirmComponent } from '../modals/confirm/modal-confirm.component';
import { Injectable } from '@angular/core';
import { ModalErrorComponent } from '../modals/error/modal-error.component';
import { ModalSuccessComponent } from '../modals/success/modal-success.component';

@Injectable({
  providedIn: null
})

export class ModalService {

  constructor(private modal: NgbModal) {
  }

  openModalConfirm(title, content, ok?, cancel?) {
    const modalRef = this.modal.open(ModalConfirmComponent, {
      keyboard: false,
      backdrop: 'static'
    });
    modalRef.componentInstance.modalTitle = title;
    modalRef.componentInstance.modalContent = content;
    modalRef.result.then(value => {
      (value) ? ok() : cancel();
    })
  }

  openModalError(title, content, btnTitle, ok?) {
    if (this.modal.hasOpenModals()) {
      this.modal.dismissAll();
    }

    const modalRef = this.modal.open(ModalErrorComponent, {
      keyboard: false,
      backdrop: 'static'
    });
    modalRef.componentInstance.modalTitle = title;
    modalRef.componentInstance.modalContent = content;
    modalRef.componentInstance.buttonTitle = btnTitle;
    modalRef.result.then(() => {
      ok();
    })
  }

  openModalDelete(title, content, result, btnTitle, ok?) {
    if (this.modal.hasOpenModals()) {
      return;
    }

    const modalRef = this.modal.open(ModalErrorComponent, {
      keyboard: false,
      backdrop: 'static'
    });
    modalRef.componentInstance.modalTitle = title;
    modalRef.componentInstance.modalContent = result + content;
    modalRef.componentInstance.buttonTitle = btnTitle;
    modalRef.result.then(() => {
      ok();
    })
  }

  openModalSuccess(title, content, btnTitle, ok?) {
    if (this.modal.hasOpenModals()) {
      return;
    }
    const modalRef = this.modal.open(ModalSuccessComponent, {
      keyboard: false,
      backdrop: 'static'
    });
    modalRef.componentInstance.modalTitle = title;
    modalRef.componentInstance.modalContent = content;
    modalRef.componentInstance.buttonTitle = btnTitle;
    modalRef.result.then(() => {
      ok();
    })
  }
}
