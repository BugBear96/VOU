import { Component, OnInit, Injector } from '@angular/core';
//import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { MatDialog } from '@angular/material';
//import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { AppComponentBase } from '@shared/app-component-base';
import { finalize } from 'rxjs/operators'
import {
    VoucherServiceProxy,
    ListResultDtoOfVoucherPlatformListDto,
    EntityDto,
    VoucherPlatformListDto
} from '@shared/service-proxies/service-proxies';

import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { AppConsts } from '../../../shared/AppConsts';


import { UpdateBannerComponent } from './update-banner/update-banner.component';
//import { EditStateComponent } from './edit-state/edit-state.component';


@Component({
    //selector: 'app-voucher-platform',
    templateUrl: './voucher-platform.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
    ],
})
export class VoucherPlatformComponent extends AppComponentBase implements OnInit {

    voucherPlatforms: VoucherPlatformListDto[] = [];
    keyword = '';
    showArchived = false;
    createPermission = false;
    editPermission = false;
    imgUrl = '';

    constructor(
        injector: Injector,
        private _voucherService: VoucherServiceProxy,
        private _dialog: MatDialog,
        //private route: ActivatedRoute,
        //private router: Router,
        private _permissionChecker: PermissionCheckerService,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.imgUrl = AppConsts.remoteServiceBaseUrl;
        this.load();
    }

    load() {
        //console.dir(this.showArchived);

        this._voucherService
            .getVoucherPlatforms(this.keyword, this.showArchived)
            //.getTenantCategories(this.keyword, this.isActive)
            .subscribe((result: ListResultDtoOfVoucherPlatformListDto) => {
                this.voucherPlatforms = result.items;
                //console.dir(this.voucherPlatforms);
            });
        this.createPermission = this._permissionChecker.isGranted('VoucherPlatform.Create');
        this.editPermission = this._permissionChecker.isGranted('VoucherPlatform.Edit');
    }

    updateCover(platform): void {
        let createOrEditDialog;

        createOrEditDialog = this._dialog.open(UpdateBannerComponent, {
            data: platform.id
        });

        createOrEditDialog.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });

    }

    toggleArchived(e: any, platform: any): void {

        abp.message.confirm(
            this.l('UserDeleteWarningMessage', platform.name),
            (result: boolean) => {
                if (result) {
                    let input = new EntityDto();
                    input.id = platform.id;
                    if (e.checked) {
                        this._voucherService
                            .archiveVoucherPlatform(input)
                            .pipe(finalize(() => this.load()))
                            .subscribe((response: any) => {
                                //this.load();
                            }, err => {
                                //handle the error
                            });
                    } else {
                        this._voucherService
                            .activateVoucherPlatform(input)
                            .pipe(finalize(() => this.load()))
                            .subscribe((response: any) => {
                                //this.load();
                            }, err => {
                                //handle the error
                            });

                    }
                    
                    
                    //abp.message.info("True");

                }
            }
        );
       
        /*
        abp.message.confirm(
            this.l('UserDeleteWarningMessage', platform.name),
            (result: boolean) => {
                if (result) {
                    abp.message.info("True");
                    
                }
            }
        );
        */
    }

}
