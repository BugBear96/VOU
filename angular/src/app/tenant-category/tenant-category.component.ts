import { Component, OnInit, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
//import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { AppComponentBase } from '@shared/app-component-base';

import {
    TenantCategoryServiceProxy,
    ListResultDtoOfTenantCategoryListDto,
    GetUpdateIsActiveInput,
    TenantCategoryListDto
} from '@shared/service-proxies/service-proxies';

import { CreateCategoryComponent } from './create-category/create-category.component';
import { EditCategoryComponent } from './edit-category/edit-category.component';

@Component({
    //selector: 'app-tenant-category',
    templateUrl: './tenant-category.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
    ]
})
export class TenantCategoryComponent extends AppComponentBase implements OnInit {

    tenantCategory: TenantCategoryListDto[] = [];
    keyword = '';
    isActive: boolean | null;

    constructor(
        injector: Injector,
        private _tenantCategoryService: TenantCategoryServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    ngOnInit() {
        this.load();
    }

    load() {
        this._tenantCategoryService
            .getTenantCategories(this.keyword, this.isActive)
            .subscribe((result: ListResultDtoOfTenantCategoryListDto) => {
                this.tenantCategory = result.items;
                //console.dir(this.tenantCategory);
            });
    }

    createCategory(): void {
        this.showCreateOrEditDialog();
    }

    editCategory(category: TenantCategoryListDto): void {
        this.showCreateOrEditDialog(category.id);
    }

    showCreateOrEditDialog(id?: number): void {
        let createOrEditDialog;
        if (id === undefined || id <= 0) {
            createOrEditDialog = this._dialog.open(CreateCategoryComponent);
        } else {
            createOrEditDialog = this._dialog.open(EditCategoryComponent, {
                data: id
            });
        }

        createOrEditDialog.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });
    }

    activate(category: TenantCategoryListDto): void {
        this._tenantCategoryService
            .updateIsActive(new GetUpdateIsActiveInput({ categoryId: category.id, isActive: !category.isActive }))
            .subscribe((result: any) => {
                //this.tenantCategory = result.items;
                //console.dir(result);
                if (result.reasonPhrase == 'OK')
                    this.load();
            });
    }

}
