import { Component, OnInit, Injector } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material/chips';


import {
    TenantCategoryEditDto,
    //TenantSubCategoryDto,
    TenantSubCategory,
    TenantCategoryServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
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

export class CreateCategoryComponent extends AppComponentBase implements OnInit {
    saving = false;
    category: TenantCategoryEditDto = new TenantCategoryEditDto();

    constructor(
        injector: Injector,
        public _tenantCategoryService: TenantCategoryServiceProxy,
        private _dialogRef: MatDialogRef<CreateCategoryComponent>
    ) {
        super(injector);
    }

    ngOnInit() {
        this.category.subCategories = [];
    }

    save(): void {
        this.saving = true;
        this.category.subCategories = this.subcategories;
        this.category.isActive = true;
        this._tenantCategoryService
            .createOrUpdateTenantCategory(this.category)
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

    visible = true;
    selectable = true;
    removable = true;
    addOnBlur = true;
    readonly separatorKeysCodes: number[] = [ENTER, COMMA];
    subcategories: TenantSubCategory[] = [
    ];

    add(event: MatChipInputEvent): void {
        const input = event.input;
        const value = event.value;

        // Add our fruit
        if ((value || '').trim()) {
            let subcategory = new TenantSubCategory();
            subcategory.title = value.trim();
            this.subcategories.push(subcategory);
        }

        // Reset the input value
        if (input) {
            input.value = '';
        }
        //console.dir(this.subcategories);
    }

    remove(subcategory: TenantSubCategory): void {
        const index = this.subcategories.indexOf(subcategory);

        if (index >= 0) {
            this.subcategories.splice(index, 1);
        }

        //console.dir(this.subcategories);
    }


}
