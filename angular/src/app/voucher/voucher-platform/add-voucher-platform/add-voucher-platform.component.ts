import { Component, Injector, OnInit } from '@angular/core';
import { Location } from '@angular/common'
import { finalize } from 'rxjs/operators';
//import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import {
    VoucherServiceProxy,
    VoucherPlatformEditDto,

} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'app-add-voucher-platform',
    templateUrl: './add-voucher-platform.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
      mat-form-field {
        width: 100%;
      }
      mat-checkbox {
        padding-bottom: 5px;
      }
    `
    ]
})
export class AddVoucherPlatformComponent extends AppComponentBase implements OnInit {

    saving = false;
    platform: VoucherPlatformEditDto = new VoucherPlatformEditDto();


    constructor(
        injector: Injector,
        private _voucherService: VoucherServiceProxy,
        private _location: Location
    ) {
        super(injector);
    }

  ngOnInit() {
  }

    save(): void {
        this.saving = true;

        //this.user.roleNames = this.getCheckedRoles();

        this._voucherService
            .createOrUpdateVoucherPlatform(this.platform)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                //this.close(true);
            });
    }

}
