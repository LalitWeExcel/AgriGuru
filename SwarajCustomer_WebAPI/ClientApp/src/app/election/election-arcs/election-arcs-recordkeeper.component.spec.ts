import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectionArcsRecordkeeperComponent } from './election-arcs-recordkeeper.component';

describe('ElectionArcsRecordkeeperComponent', () => {
  let component: ElectionArcsRecordkeeperComponent;
  let fixture: ComponentFixture<ElectionArcsRecordkeeperComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ElectionArcsRecordkeeperComponent]
    });
    fixture = TestBed.createComponent(ElectionArcsRecordkeeperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
