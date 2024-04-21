import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditOfficerAuditListComponent } from './audit-officer-audit-list.component';

describe('AuditOfficerAuditListComponent', () => {
  let component: AuditOfficerAuditListComponent;
  let fixture: ComponentFixture<AuditOfficerAuditListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditOfficerAuditListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuditOfficerAuditListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
