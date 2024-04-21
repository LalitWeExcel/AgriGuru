import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectionVerificationRecordkeeparComponent } from './election-verification-recordkeepar.component';

describe('ElectionVerificationRecordkeeparComponent', () => {
  let component: ElectionVerificationRecordkeeparComponent;
  let fixture: ComponentFixture<ElectionVerificationRecordkeeparComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ElectionVerificationRecordkeeparComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ElectionVerificationRecordkeeparComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
