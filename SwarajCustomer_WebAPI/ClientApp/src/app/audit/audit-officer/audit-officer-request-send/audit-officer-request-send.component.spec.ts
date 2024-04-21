import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditOfficerRequestSendComponent } from './audit-officer-request-send.component';

describe('AuditOfficerRequestSendComponent', () => {
  let component: AuditOfficerRequestSendComponent;
  let fixture: ComponentFixture<AuditOfficerRequestSendComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditOfficerRequestSendComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AuditOfficerRequestSendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
