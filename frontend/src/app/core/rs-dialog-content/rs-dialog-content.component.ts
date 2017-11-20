import {Input, Component} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

export enum DialogType {
    ALERT,
    CONFIRM
}

export interface DialogTypeOptions {
    type?: DialogType;
    title?: string;
    message?: string;
    canCancel?: boolean;
}

@Component({
    selector: 'rs-dialog-content-component',
    templateUrl: './rs-dialog-content.component.html'
})
export class RSDialogContentComponent {
    @Input() options: DialogTypeOptions = {
        type: DialogType.ALERT,
        canCancel: true
    };

    constructor(public modalDialog: NgbActiveModal) {
    }

    close(reason?: any) {
        this.modalDialog.close(reason);
    }

    cancel(reason: any = 'cancel') {
        this.modalDialog.dismiss(reason);
    }
}