import { Component, OnInit, Injector, Optional, Inject } from '@angular/core';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material/chips';

import {
    BranchServiceProxy,
    StateEditDto,
    City
} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'app-edit-state',
    templateUrl: './edit-state.component.html',
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
export class EditStateComponent extends AppComponentBase implements OnInit {

    saving = false;
    state: StateEditDto = new StateEditDto();

    constructor(
        injector: Injector,
        public _branchService: BranchServiceProxy,
        private _dialogRef: MatDialogRef<EditStateComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
    ) {
        super(injector);
    }

    ngOnInit() {
        this.state.cities = [];
        //this.category.isActive = true;
        this._branchService
            .getStateForEdit(this._id)
            .subscribe((result: StateEditDto) => {
                this.state = result;
                this.cities = result.cities;
            });
    }

    save(): void {
        this.saving = true;
        this.state.cities = this.cities;

        this._branchService
            .createOrUpdateState(this.state)
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
    cities: City[] = [
    ];

    add(event: MatChipInputEvent): void {
        const input = event.input;
        const value = event.value;

        // Add our fruit
        if ((value || '').trim()) {
            let city = new City();
            city.cityName = value.trim();
            this.cities.push(city);
        }

        // Reset the input value
        if (input) {
            input.value = '';
        }
        //console.dir(this.subcategories);
    }

    remove(city: City): void {
        const index = this.cities.indexOf(city);

        if (index >= 0) {
            this.cities.splice(index, 1);
        }

        console.dir(this.cities);
    }

}
