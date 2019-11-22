import { Component, Injector, OnInit, ViewChild } from '@angular/core';
//import { MatDialogRef, MatCheckboxChange } from '@angular/material';
import { finalize } from 'rxjs/operators';
//import * as _ from 'lodash';
import { Location } from '@angular/common'
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { ActivatedRoute } from "@angular/router";
import { TermConditionsComponent } from '../term-conditions/term-conditions.component';
import {
    VoucherServiceProxy,
    VoucherPlatformEditDto,
    VoucherSettings,
    VoucherTermCondition
} from '@shared/service-proxies/service-proxies';


@Component({
    selector: 'app-edit-voucher-platform',
    templateUrl: './edit-voucher-platform.component.html',
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
export class EditVoucherPlatformComponent extends AppComponentBase implements OnInit {
    saving = false;
    platform: VoucherPlatformEditDto = new VoucherPlatformEditDto();
    _id: number;
    @ViewChild(TermConditionsComponent, { static: false })
    private termConditionsComponent: TermConditionsComponent;

    /*
    {
    "Rules": [{
        "Days": [1, 2, 3, 4, 5],
        "CalendarPeriodIds": [],
        "FlatRates": [{ "Begin": 0, "End": 10, "Fee": 3.0 }, { "Begin": 18, "End": 24, "Fee": 3.0 }],
        "HourlyRates": [{ "Unit": 1.0, "Fee": 1.0 }, { "Unit": 2.0, "Fee": 3.0 }, { "Unit": 1.0, "Fee": 1.0 }],
        "MaxDailyFee": 8.0, "GracePeriodMinute": 15
    },
        { "Days": [6, 0], "CalendarPeriodIds": [1], "FlatRates": [], "HourlyRates": [{ "Unit": 2.0, "Fee": 3.0 }, { "Unit": 1.0, "Fee": 1.0 }], "MaxDailyFee": 12.0, "GracePeriodMinute": 15 }]
}*/
    constructor(
        injector: Injector,
        private _voucherService: VoucherServiceProxy,
        private route: ActivatedRoute,
        private _location: Location
    ) {
        super(injector);
        //this._id = 1;
    }

    ngOnInit() {
        this._id = this.route.snapshot.paramMap.has("id") ? parseInt(this.route.snapshot.paramMap.get("id")) : 0;
        //console.log(this._id)
        this.load()
        //console.log(this.platform.termConditionJson.init());
    }

    load(): void {
        this._voucherService
            .getVoucherPlatformForEdit(this._id)
            .subscribe((result: VoucherPlatformEditDto) => {
                this.platform = result;
                //console.log(this.platform.termConditionJson)
                //console.dir(this.voucherPlatforms);
            });
    }

    save(): void {
        this.saving = true;

        
        this.platform.termConditionJson = this.termConditionsComponent.output();
        
        console.log(this.platform.termConditionJson);
        //this.user.roleNames = this.getCheckedRoles();
        //return
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
