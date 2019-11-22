import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  TenantServiceProxy,
  TenantDto
} from '@shared/service-proxies/service-proxies';

import { FormControl, FormGroup } from '@angular/forms';
import * as _ from 'lodash';
import {
    TenantCategoryServiceProxy,
    ListResultDtoOfTenantCategoryListDto,
    TenantCategoryListDto
} from '@shared/service-proxies/service-proxies';


@Component({
  templateUrl: 'edit-tenant-dialog.component.html',
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
export class EditTenantDialogComponent extends AppComponentBase
    implements OnInit {

    saving = false;
    tenant: TenantDto = new TenantDto();
    tenantCategories: TenantCategoryListDto[] = [];

    subCategories = [];
    
    editForm: FormGroup;

    constructor(
        injector: Injector,
        public _tenantService: TenantServiceProxy,
        private _tenantCategoryService: TenantCategoryServiceProxy,
        private _dialogRef: MatDialogRef<EditTenantDialogComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.editForm = new FormGroup({
            selectedSubCategories: new FormControl()
        });
        /*
        this._tenantService.get(this._id).subscribe((result: TenantDto) => {
            this.tenant = result;
            console.dir(this.tenant)
        });
        */
        this._tenantService.getTenant(this._id).subscribe((result: TenantDto) => {
            this.tenant = result;
            this.chooseCategory();
            //console.dir(this.tenant)
        });

        

      
    }

    chooseCategory(): void {
        this._tenantCategoryService
            .getTenantCategories('', true)
            .subscribe((result: ListResultDtoOfTenantCategoryListDto) => {
                this.tenantCategories = result.items;
                for (var tenantCategory of this.tenantCategories) {
                    this.tenant.category.title == tenantCategory.title ? this.tenant.category = tenantCategory : null;
                    this.tenant.category.title == tenantCategory.title ? this.changeCategory(tenantCategory) : null;
                }
                //console.dir(this.tenantCategory);
            });
    }

    save(): void {
        this.tenant.subCategories = this.editForm.value.selectedSubCategories;
        //console.dir(this.editForm.value.selectedSubCategories);
        //console.dir(this.tenant);
        
        this.saving = true;

        this._tenantService
          .updateTenant(this.tenant)
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

    changeCategory(category: TenantCategoryListDto): void {
        this.subCategories = category.subCategories;
        let selected = [];
        
        let subCategories = [];
        _.forEach(this.tenant.subCategories, function (value, key) {
            subCategories.push({ title: value.title });
        });
        console.dir(subCategories)
        _.forEach(this.subCategories, function (value, key) {
            //console.dir(value)
            //console.dir(this.subCategories);
            let data = subCategories.find(ob => ob.title === value.title);
            if (data != null) {
                console.dir(value)
                //this.editForm
                //this.editForm.value.
                //this.editForm.controls.selectedSubCategories.setValue(value);
                //this.editForm.get('selectedSubCategories').setValue(value);
                selected.push(value);
                //this.editForm.value.selectedSubCategories.push(value);
            }

            //console.dir(this.selectedSubCategories)
            //selected.cityName == subcategory.title ? this.location.city = city : null;

        });
        this.editForm.setValue({ selectedSubCategories: selected });
        //console.dir(subCategories);
        //let selected = this.location.city;
        //this.tenant.s = null;
        //console.dir(this.subCategories)
        //setTimeout(() => {    //<<<---    using ()=> syntax
        //    console.dir(this.subCategories)
        //}, 3000);



        
        /*
        this.tenant.subCategories.forEach()
        for (var subcategory of this.subCategories) {
            selected.cityName == subcategory.title ? this.location.city = city : null;
        }
        */
        //console.dir(category.subCategories)
    }
}
