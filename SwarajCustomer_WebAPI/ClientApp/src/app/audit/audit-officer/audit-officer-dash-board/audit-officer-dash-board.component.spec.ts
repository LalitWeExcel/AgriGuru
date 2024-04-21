import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditOfficerDashBoardComponent } from './audit-officer-dash-board.component';

describe('AuditOfficerDashBoardComponent', () => {
  let component: AuditOfficerDashBoardComponent;
  let fixture: ComponentFixture<AuditOfficerDashBoardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditOfficerDashBoardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuditOfficerDashBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
