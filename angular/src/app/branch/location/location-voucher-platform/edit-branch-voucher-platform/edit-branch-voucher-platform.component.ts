import { Component, OnInit, Injector, Optional, Inject } from '@angular/core';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';

import {
    BranchServiceProxy,
    BranchWithVoucherPlatformEditDto
} from '@shared/service-proxies/service-proxies';

import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';

// Depending on whether rollup is used, moment needs to be imported differently.
// Since Moment.js doesn't have a default export, we normally need to import using the `* as`
// syntax. However, rollup creates a synthetic default module and we thus need to import it using
// the `default as` syntax.


// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};


@Component({
  selector: 'app-edit-branch-voucher-platform',
  templateUrl: './edit-branch-voucher-platform.component.html',
    //animations: [appModuleAnimation()],
    styles: [
        `
      mat-form-field {
        width: 100%;
      }
      mat-checkbox {
        padding-bottom: 5px;
      }
    `
    ],
providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},

    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},

    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }
  ],
})
export class EditBranchVoucherPlatformComponent extends AppComponentBase implements OnInit {

    platform: BranchWithVoucherPlatformEditDto = new BranchWithVoucherPlatformEditDto();

    saving = false;

    start = new FormControl();
    end = new FormControl();

    constructor(
        injector: Injector,
        public _branchService: BranchServiceProxy,
        private _dialogRef: MatDialogRef<EditBranchVoucherPlatformComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _id: number,

        //private _voucherService: VoucherServiceProxy,
        //private route: ActivatedRoute
    ) {
        super(injector);
        //this._id = 1;
    }


    ngOnInit() {
        this.load();


    }

    load(): void {

        this._branchService
            .getVoucherPlatformForEdit(this._id)
            .subscribe((result: BranchWithVoucherPlatformEditDto) => {
                this.platform = result;
                this.start = new FormControl( this.platform.start );
                this.end= new FormControl( this.platform.end );
                //console.log(new Date(parseInt(this.platform.start.format('YYYY')), parseInt(this.platform.start.format('MM')), parseInt(this.platform.start.format('DD'))))
                //this.loadStates();
                //console.log(parseInt(this.platform.start.format('MM')))
            });
    }

    close(result: any): void {
        this._dialogRef.close(result);
    }

    save(): void {
        if (this.start.value > this.end.value) {
            this.notify.error(this.l('Failed! The End Date Must Be Larger Than Start Date!'));
            return
        }
            //return

        this.saving = true;
        //console.log(this.start.value.toISOString());
        //console.log(this.end.value.toISOString());
        this.platform.start = this.start.value.add(8, 'hours')
        this.platform.end = this.end.value.add(8, 'hours')
        
        this._branchService
            .createOrUpdateVoucherPlatform(this.platform)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close(true);
            });

    }

}
