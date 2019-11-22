import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateBranchCoverComponent } from './update-branch-cover.component';

describe('UpdateBranchCoverComponent', () => {
  let component: UpdateBranchCoverComponent;
  let fixture: ComponentFixture<UpdateBranchCoverComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateBranchCoverComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateBranchCoverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
