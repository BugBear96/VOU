import { Component, OnInit, Input, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import {
    BranchServiceProxy,
    LocationListDto,
    ListResultDtoOfLocationListDto,
    LocationEditDto,
    ListResultDtoOfStateListDto,
    StateListDto,
    StateDto,
    CityDto,
    WorkingHourSettings,
    Days,
    TimeTable,
    TimeTableSettings
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '../../../../shared/AppConsts';
import { UpdateBranchCoverComponent } from '../update-branch-cover/update-branch-cover.component';
import * as _ from 'lodash';


@Component({
    selector: 'app-location-details',
    templateUrl: './location-details.component.html',
    styleUrls: ['./location-details.component.css']
})
export class LocationDetailsComponent extends AppComponentBase implements OnInit {

    weekDaysLabel = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    location: LocationEditDto;
    //location: LocationEditDto = new LocationEditDto();
    imgUrl = '';
    timetablesJson: any;
    timetablesDescription: [] = [];
    permissions: {} = { create: false, edit: false };
    isEdit = false;
    saving = false;
    states: StateListDto[] = [];
    cities: CityDto[] = [];
    @Output() update = new EventEmitter();

    constructor(
        injector: Injector,
        private _dialog: MatDialog,
        private _branchService: BranchServiceProxy,
        private _permissionChecker: PermissionCheckerService,
    ) {
        super(injector);
    }

    @Input()
    set Location(location: any) {
        if (location instanceof Array)
            location = location[0]
        if (location) {
            //this.location = location;
            this._branchService
                .getLocationForEdit(location)
                .subscribe((result: LocationEditDto) => {
                    this.getTimeTableDescription(result.timeTableJson);
                    this.location = result;
                    //this.timetablesJson = JSON.parse(this.location.timeTableJson)
                    //console.log(JSON.parse(this.location.timeTableJson
                    
                    //console.log(this.location)
                    this.loadStates();
                });
        }
    }

    ngOnInit() {
        this.permissions['create'] = this._permissionChecker.isGranted('Branch.Create');
        this.permissions['edit'] = this._permissionChecker.isGranted('Branch.Edit');
        this.imgUrl = AppConsts.remoteServiceBaseUrl;
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

    edit(): void {

        this.timetablesJson = [];
        //console.log(this.createNewTimeTable())
        this.location.timeTableJson.timeTables.forEach(function (d) {
            //console.log(d)
            //console.log(this.createNewTimeTable )
            //console.log(this.createNewTimeTable(d))
            this.timetablesJson.push(this.createNewTimeTable(d))
        }, this)
        //_.forEach(this.location.timeTableJson.timeTables, );
        //console.log(this.timetablesJson)
        this.isEdit = true;
        //console.log(this.isEdit)

    }

    createNewTimeTable(t?: any) {
        if (t === undefined)
            return {
                'scopes': _.map(this.weekDaysLabel, function (d, i) {
                    return {
                        name: d,
                        value: i,
                        checked: false
                    };
                }),
                'days': [],
                'workingHours': []
            };

        let timetable = {};

        timetable['days'] = t.days;
        timetable['scopes'] = _.map(this.weekDaysLabel, function (d, i) {
            return {
                name: d,
                value: i,
                checked: _.indexOf(t.days, i) !== -1
            };
        });
        timetable['workingHours'] = _.map(t.workingHours, function (d, i) {
            return {
                begin: d.begin,
                end: d.end
            }
        });

        //console.log(timetable)
        return timetable;
    }

    getFormatedTimeTable(t: any) {
        let timetable = new TimeTable();
        timetable.days = _.chain(t.scopes).filter('checked').map('value').value();
        
        timetable.workingHours = _.map(t.workingHours, function (d) {
            let workingHour = new WorkingHourSettings();
            workingHour.begin = d.begin;
            workingHour.end = d.end;
            return workingHour;
        })
        //console.log(timetable)
        return timetable
    }

    addWorkingHour(item: any) {
        item.workingHours.push({
            begin: 8,
            end: 18
        });
    }

    removeWorkingHour(x: number, y: number) {
        this.timetablesJson[x].workingHours.splice(y, 1);
    }

    removeTimeTable(x: number) {
        this.timetablesJson.splice(x, 1);
    }

    addTimeTable() {
        this.timetablesJson.push(this.createNewTimeTable());
    }

    myOnChange(event: any) {
        //console.log(event)
    }

    myOnUpdate(event: any) {
        //console.log(event)
    }

    myOnFinish(event: any, x: number, y: number) {
        //console.log(event)
        //console.log(x)
        //console.log(y)
        this.timetablesJson[x]['workingHours'][y] = { begin: event.from, end: event.to }
    }

    getTimeTableDescription(timetables: any) {
        //console.log(timetables)
        //if(!timetables) return
        function getScope(timetable: any, global: any) {
            let dayScope;
            let days = _.chain(timetable.days).sortBy().value();
            if (days.length === global.weekDaysLabel.length) {
                dayScope = ['Everyday'];
            } else {
                let dayRange = [];
                _.forEach(days, function (d) {
                    let lastDayRange = dayRange.length ? dayRange[dayRange.length - 1] : null;
                    if (lastDayRange && d === (lastDayRange.stop + 1)) {
                        lastDayRange.stop = d;
                    } else {
                        lastDayRange = {
                            start: d,
                            stop: d
                        };
                        dayRange.push(lastDayRange);
                    }
                });

                if (dayRange.length > 1) {
                    const firstRange = dayRange[0];
                    const lastRange = dayRange[dayRange.length - 1];
                    if (((lastRange.stop + 1) % 7) === firstRange.start) {
                        lastRange.stop = firstRange.stop;
                        dayRange.splice(0, 1);
                    }
                }

                dayScope = _.map(dayRange, function (d) {
                    if (d.start === d.stop) return global.weekDaysLabel[d.start];
                    return global.weekDaysLabel[d.start] + ' - ' + global.weekDaysLabel[d.stop];
                });
            }

            return dayScope;
        }

        function getWorkingHourDescription(begin: string, end: string) {
            return 'Open from ' + begin + ':00 to ' + end + ':00'
        }
        this.timetablesDescription = [];
        //this.timetablesDescription = [];
        timetables.timeTables.forEach(function (d) {
            let description = {}
            description['scope'] = getScope(d, this);
            //console.log(description['scope']);
            description['details'] = _.map(d.workingHours, function (d) {
                return getWorkingHourDescription(d.begin, d.end)
            })
            this.timetablesDescription.push(description)

            //console.log(this.timetablesDescription)
        }, this)
    }

    getWorkingHourDescription(x: number, y: number) {
        return 'Open from ' + this.timetablesJson[x]['workingHours'][y].begin + ':00 to ' + this.timetablesJson[x]['workingHours'][y].end + ':00'
    }

    

    save(): void {
        //console.log(this.timetablesJson)
        this.location.timeTableJson.timeTables = [];
        this.timetablesJson.forEach(function (d) {
            //console.log(d)
            //console.log(this.createNewTimeTable )
            //console.log(this.createNewTimeTable(d))
            this.location.timeTableJson.timeTables.push(this.getFormatedTimeTable(d))
        }, this)
        //return
        this.saving = true;
        /*
        this.location.timeTableJson = new TimeTableSettings({
            timeTables: [new TimeTable({
                days: [0,1,2,3,4],
                workingHours: [new WorkingHourSettings({ begin: 8, end: 22 })]
            })]
        })
        */

        //console.log(this.location)
        this._branchService
            .createOrUpdateLocation(this.location)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.update.emit();
                this.isEdit = false;
            });
    }

    cancel(): void {
        this.isEdit = false;
    }

    updateCover(location): void {
        let createOrEditDialog;

        createOrEditDialog = this._dialog.open(UpdateBranchCoverComponent, {
            data: location.id
        });

        createOrEditDialog.afterClosed().subscribe(result => {
            if (result) {
                //console.log(location.id)
                this.update.emit();
                //this.load();
            }
        });

    }
}
