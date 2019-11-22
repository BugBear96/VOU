import { Component, OnInit, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
//import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ActivatedRoute } from "@angular/router";
import { AppComponentBase } from '@shared/app-component-base';

import {
    BranchServiceProxy,
    //LocationListDto,
    ListResultDtoOfBranchWithVoucherPlatformListDto,
    BranchWithVoucherPlatformListDto
    //StateListDto,
} from '@shared/service-proxies/service-proxies';



import { CreateBranchVoucherPlatformComponent } from './create-branch-voucher-platform/create-branch-voucher-platform.component';
import { EditBranchVoucherPlatformComponent } from './edit-branch-voucher-platform/edit-branch-voucher-platform.component';
import { AppConsts } from '../../../../shared/AppConsts';

@Component({
    //selector: 'app-location-voucher-platform',
    templateUrl: './location-voucher-platform.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
    ]
})
export class LocationVoucherPlatformComponent extends AppComponentBase implements OnInit {

    _id: number;
    imgUrl = '';
    voucherPlatformsFromBranch: BranchWithVoucherPlatformListDto[] = [];
    //saving = false;

    constructor(
        injector: Injector,
        public _branchService: BranchServiceProxy,
        private _dialog: MatDialog,
        
        private route: ActivatedRoute
    ) {
        super(injector);
    }

    ngOnInit() {
        this.imgUrl = AppConsts.remoteServiceBaseUrl;
        this.load();
        

    }

    load(): void {
        this._id = parseInt(this.route.snapshot.paramMap.get("id"));
        //console.log(123)
        

        this._branchService
            .getVoucherPlatformByBranch(this._id)
            .subscribe((result: ListResultDtoOfBranchWithVoucherPlatformListDto) => {
                this.voucherPlatformsFromBranch = result.items;
                //console.dir(this.voucherPlatformsFromBranch);
            });
    }

    create(): void {
        this.showCreateOrEditDialog();
    }

    edit(platform: BranchWithVoucherPlatformListDto): void {
        //console.dir('Edit')
        this.showCreateOrEditDialog(platform.id);
    }

    showCreateOrEditDialog(id?: number): void {
        
        let createOrEditDialog;
        if (id === undefined || id <= 0) {
            createOrEditDialog = this._dialog.open(CreateBranchVoucherPlatformComponent, {
                   data: this._id
            });
        } else {
            createOrEditDialog = this._dialog.open(EditBranchVoucherPlatformComponent, {
                data: id
            });
        }

        createOrEditDialog.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });
        
    }

}
