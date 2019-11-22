import { Component, OnInit, Injector, Optional, Inject } from '@angular/core';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

import {
    BranchServiceProxy,
    LocationEditDto,
    ListResultDtoOfStateListDto,
    StateListDto,
    StateDto,
    CityDto
} from '@shared/service-proxies/service-proxies';

@Component({
  //selector: 'app-edit-location',
  templateUrl: './edit-location.component.html',
  styles: []
})
export class EditLocationComponent extends AppComponentBase implements OnInit {

    saving = false;
    location: LocationEditDto = new LocationEditDto();
    states: StateListDto[] = [];
    cities: CityDto[] = [];

    constructor(
        injector: Injector,
        public _branchService: BranchServiceProxy,
        private _dialogRef: MatDialogRef<EditLocationComponent>,
        @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
    ) {
        super(injector);
    }

    ngOnInit() {
        this.load();
    }

    load(): void {

        this._branchService
            .getLocationForEdit(this._id)
            .subscribe((result: LocationEditDto) => {
                this.location = result;
                this.loadStates();
            });
    }

    save(): void {
        this.saving = true;
        this._branchService
            .createOrUpdateLocation(this.location)
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

    loadStates(): void {
        this._branchService
            .getStates('')
            .subscribe((result: ListResultDtoOfStateListDto) => {
                this.states = result.items;
                for (var state of this.states) {
                    this.location.state.stateName == state.stateName ? this.location.state = state : null;
                    this.location.state.stateName == state.stateName ? this.changeCities(state) : null;
                }
            });
    }

    changeCities(state: StateListDto): void {
        this.cities = state.cities;
        let selected = this.location.city;
        this.location.city = null;
        for (var city of this.cities) {
            selected.cityName == city.cityName ? this.location.city = city : null;
        }

    }

    close(result: any): void {
        this._dialogRef.close(result);
    }

}
