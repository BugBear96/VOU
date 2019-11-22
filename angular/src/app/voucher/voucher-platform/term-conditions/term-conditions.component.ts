import { Component, OnInit, Input } from '@angular/core';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material/chips';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import {
    VoucherSettings,
    VoucherTermCondition
} from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-term-conditions',
  templateUrl: './term-conditions.component.html',
    styleUrls: ['./term-conditions.component.css'],
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
export class TermConditionsComponent implements OnInit {

    arrangeRules = false;
    
    constructor() { }

    ngOnInit() {
    }

    @Input()
    set termConditionsJsons(termConditions: any) {
        
        if (termConditions && termConditions.termConditions[0].terms.length)
            this.rules = termConditions.termConditions[0].terms;
        else
            this.rules = []
    }

    output() {
        return new VoucherSettings({
            'termConditions': [new VoucherTermCondition({ 'terms': this.rules })]
        });
    }

    visible = true;
    selectable = true;
    removable = true;
    addOnBlur = true;
    readonly separatorKeysCodes: number[] = [ENTER, COMMA];
    rules: any = [
    ];

    add(event: MatChipInputEvent): void {
        const input = event.input;
        const value = event.value;

        // Add our fruit
        if ((value || '').trim()) {
            let rule = value.trim();
            this.rules.push(rule);
        }

        // Reset the input value
        if (input) {
            input.value = '';
        }
    }

    remove(rule: any): void {
        const index = this.rules.indexOf(rule);

        if (index >= 0) {
            this.rules.splice(index, 1);
        }
    }

    drop(event: CdkDragDrop<string[]>) {
        moveItemInArray(this.rules, event.previousIndex, event.currentIndex);
    }

}
