import { Component, OnInit, Injector } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

import {
    BranchServiceProxy,
    LocationEditDto,
    ListResultDtoOfStateListDto,
    StateListDto,
    CityDto
} from '@shared/service-proxies/service-proxies';

@Component({
  //selector: 'app-create-location',
  templateUrl: './create-location.component.html',
  styles: [

  ]
})
export class CreateLocationComponent extends AppComponentBase implements OnInit {

    saving = false;
    location: LocationEditDto = new LocationEditDto();
    states: StateListDto[] = [];
    cities: CityDto[] = [];

    constructor(
        injector: Injector,
        public _branchService: BranchServiceProxy,
        private _dialogRef: MatDialogRef<CreateLocationComponent>
    ) {
        super(injector);
    }

    ngOnInit() {
        this.load();
    }

    load(): void {
        this._branchService
            .getStates('')
            .subscribe((result: ListResultDtoOfStateListDto) => {
                //console.dir(result);
                this.states = result.items;

            });
    }

    changeCities(state: StateListDto): void {
        this.cities = state.cities;
        this.location.city = null;
    }

    save(): void {
        this.saving = true;
        //console.dir(this.location)
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

    close(result: any): void {
        this._dialogRef.close(result);
    }

}
