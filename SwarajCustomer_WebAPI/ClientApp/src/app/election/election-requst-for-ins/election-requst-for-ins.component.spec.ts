import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectionRequstForInsComponent } from './election-requst-for-ins.component';

describe('ElectionRequstForInsComponent', () => {
  let component: ElectionRequstForInsComponent;
  let fixture: ComponentFixture<ElectionRequstForInsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ElectionRequstForInsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ElectionRequstForInsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
