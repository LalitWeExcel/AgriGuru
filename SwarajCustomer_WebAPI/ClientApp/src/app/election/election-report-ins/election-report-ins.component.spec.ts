import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectionReportInsComponent } from './election-report-ins.component';

describe('ElectionReportInsComponent', () => {
  let component: ElectionReportInsComponent;
  let fixture: ComponentFixture<ElectionReportInsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ElectionReportInsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ElectionReportInsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
