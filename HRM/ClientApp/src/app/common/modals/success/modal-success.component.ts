import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
    selector: 'app-modal-success',
    templateUrl: './modal-success.component.html',
    styleUrls: ['./modal-success.component.css']
})
export class ModalSuccessComponent implements OnInit {

    @Input() modalTitle;
    @Input() modalContent;
    @Input() optionModalContent;
    @Input() buttonTitle;
    @Input() url;

    constructor(public activeModal: NgbActiveModal, private _router: Router) {
    }

    ngOnInit() {
    }

    reLoad() {
        if (this.url != null) {
            this.activeModal.dismiss(this.url);
            this._router.navigateByUrl(this.url);
        } else {
            this.activeModal.close();
        }
    }
}
