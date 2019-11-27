import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
//import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { AppComponentBase } from '@shared/app-component-base';

import {
    BranchServiceProxy,
    LocationListDto,
    ListResultDtoOfLocationListDto,
    //StateListDto,
    
} from '@shared/service-proxies/service-proxies';

import { CreateLocationComponent } from './create-location/create-location.component';
import { EditLocationComponent } from './edit-location/edit-location.component';
import { UpdateBranchCoverComponent } from './update-branch-cover/update-branch-cover.component';
import { AppConsts } from '../../../shared/AppConsts';
import * as _ from 'lodash';
import { timeout } from 'rxjs/operators';

@Component({
    //selector: 'app-location',
    templateUrl: './location.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
    ]
})
export class LocationComponent extends AppComponentBase implements OnInit {

    @ViewChild("locationSelections", { static: true }) locationSelections;
    //@ViewChild("shoes", { static: true }) shoes;
    selectedLocation: number[] = [];
    selected: any;
    keyword = '';
    imgUrl = '';
    locations: LocationListDto[] = [];

    constructor(
        injector: Injector,
        public _branchService: BranchServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    ngOnInit() {
        this.locationSelections.selectedOptions._multiple = false;
        this.imgUrl = AppConsts.remoteServiceBaseUrl;
        this.load();
    }

    load() {

        this.selected = this.selectedLocation.length > 0 ? this.selectedLocation.pop() : null;

        this._branchService
            .getLocations(this.keyword)
            .subscribe((result: ListResultDtoOfLocationListDto) => {
                this.locations = result.items;
                if (_.isNumber(this.selected)) {
                    setTimeout(() => {
                        this.selectedLocation.push(this.selected)
                    }, 100)

                }

            });
    }

    create(): void {
        this.showCreateOrEditDialog();
    }

    edit(location: LocationListDto): void {
        this.showCreateOrEditDialog(location.id);
    }

    showCreateOrEditDialog(id?: number): void {
        let createOrEditDialog;
        if (id === undefined || id <= 0) {
            createOrEditDialog = this._dialog.open(CreateLocationComponent);
        } else {
            createOrEditDialog = this._dialog.open(EditLocationComponent, {
                data: id
            });
        }

        createOrEditDialog.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });
    }

    updateCover(location): void {
        let createOrEditDialog;

        createOrEditDialog = this._dialog.open(UpdateBranchCoverComponent, {
            data: location.id
        });

        createOrEditDialog.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });

    }

}
