import {Injectable} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

import {
    DialogType, DialogTypeOptions,
    RSDialogContentComponent
} from '../../core/rs-dialog-content/rs-dialog-content.component';

@Injectable()
export class DialogService {

    constructor(private modalService: NgbModal) {}

    public alert(options: DialogTypeOptions | string) {
        const modalRef = this.modalService.open(RSDialogContentComponent);

        modalRef.componentInstance.options = Object.assign({
            type: DialogType.ALERT
        }, typeof(options) == "string" ? {
            canCancel: true,
            message: options
        } : options);

        return modalRef;
    }
}
