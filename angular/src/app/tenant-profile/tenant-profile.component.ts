import { Component, OnInit, Injector } from '@angular/core';

import { MatDialog } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppSessionService } from '../../shared/session/app-session.service';

import { AppComponentBase } from '@shared/app-component-base';

import { UpdateProfileComponent } from './update-profile/update-profile.component';
import { AppConsts } from '../../shared/AppConsts';
import {
    PartnerServiceProxy,
    TenantDto,
} from '@shared/service-proxies/service-proxies';


@Component({
    //selector: 'app-tenant-profile',
    templateUrl: './tenant-profile.component.html',
    animations: [appModuleAnimation()],
    styles: []
})
export class TenantProfileComponent extends AppComponentBase implements OnInit {
    
    //@ViewChild("ProfilePicture", { static: true }) profilePicture;
    imgSrc: any = "";
    constructor(
        injector: Injector,
        private _partnerService: PartnerServiceProxy,
        private _sessionService: AppSessionService,
        private _dialog: MatDialog)
    {
        super(injector);
    }

    ngOnInit(): void {
        this.load();
    }

    load(): void {

        this._partnerService
            .getTenant(this._sessionService.tenant.id)
            .subscribe((result: TenantDto) => {
                this.imgSrc = AppConsts.remoteServiceBaseUrl + "/Tenant/GetProfilePictureById?id=" + result.profilePictureId;
            });

    }
 

    updateProfilePicture(): void {
        let createOrEditDialog;
        
        createOrEditDialog = this._dialog.open(UpdateProfileComponent);

        createOrEditDialog.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });
    }
    
}
