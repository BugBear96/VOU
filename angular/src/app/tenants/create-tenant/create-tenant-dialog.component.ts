import { Component, Injector, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CreateTenantDto,
  TenantServiceProxy
} from '@shared/service-proxies/service-proxies';

import {
    TenantCategoryServiceProxy,
    ListResultDtoOfTenantCategoryListDto,
    TenantCategoryListDto
} from '@shared/service-proxies/service-proxies';

import { FormControl, FormGroup } from '@angular/forms';
import * as _ from 'lodash';

@Component({
  templateUrl: 'create-tenant-dialog.component.html',
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
export class CreateTenantDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  tenant: CreateTenantDto = new CreateTenantDto();

    editForm: FormGroup;
    tenantCategories: TenantCategoryListDto[] = [];
    subCategories = [];

    constructor(
        injector: Injector,
        public _tenantService: TenantServiceProxy,
        private _tenantCategoryService: TenantCategoryServiceProxy,
        private _dialogRef: MatDialogRef<CreateTenantDialogComponent>
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.editForm = new FormGroup({
            selectedSubCategories: new FormControl()
        });
        this.chooseCategory();
        this.tenant.isActive = true;
    }

    save(): void {
        this.tenant._subCategories = this.editForm.value.selectedSubCategories;
        this.saving = true;

        this._tenantService
          .create(this.tenant)
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

  close(result: any): void {
    this._dialogRef.close(result);
  }

    chooseCategory(): void {
        this._tenantCategoryService
            .getTenantCategories('', true)
            .subscribe((result: ListResultDtoOfTenantCategoryListDto) => {
                this.tenantCategories = result.items;
                //for (var tenantCategory of this.tenantCategories) {
                //    this.tenant.category.title == tenantCategory.title ? this.tenant.category = tenantCategory : null;
                //    this.tenant.category.title == tenantCategory.title ? this.changeCategory(tenantCategory) : null;
                //}
                //console.dir(this.tenantCategory);
            });
    }

    changeCategory(category: TenantCategoryListDto): void {
        this.subCategories = category.subCategories;
        //console.dir(this.subCategories)
        
    }


}
