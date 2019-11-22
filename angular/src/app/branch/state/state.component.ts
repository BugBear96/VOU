import { Component, OnInit, Injector } from '@angular/core';

import { MatDialog } from '@angular/material';
//import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { AppComponentBase } from '@shared/app-component-base';

import {
    BranchServiceProxy,
    ListResultDtoOfStateListDto,

    StateListDto
} from '@shared/service-proxies/service-proxies';

import { CreateStateComponent } from './create-state/create-state.component';
import { EditStateComponent } from './edit-state/edit-state.component';
    
@Component({
  //selector: 'app-state',
    templateUrl: './state.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
    ],

})
export class StateComponent extends AppComponentBase implements OnInit {

    states: StateListDto[] = [];
    keyword = '';

    constructor(
        injector: Injector,
        private _branchService: BranchServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    ngOnInit() {
        this.load();
    }

    load() {
        this._branchService
            .getStates(this.keyword)
            //.getTenantCategories(this.keyword, this.isActive)
            .subscribe((result: ListResultDtoOfStateListDto) => {
                this.states = result.items;
                //console.dir(this.states);
            });
    }

    create(): void {
        this.showCreateOrEditDialog();
    }

    edit(state: StateListDto): void {
        this.showCreateOrEditDialog(state.id);
    }

    showCreateOrEditDialog(id?: number): void {
        let createOrEditDialog;
        if (id === undefined || id <= 0) {
            createOrEditDialog = this._dialog.open(CreateStateComponent);
        } else {
            createOrEditDialog = this._dialog.open(EditStateComponent, {
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
