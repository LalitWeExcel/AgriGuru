import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectionSelectexecutiveMemberComponent } from './election-selectexecutive-member.component';

describe('ElectionSelectexecutiveMemberComponent', () => {
  let component: ElectionSelectexecutiveMemberComponent;
  let fixture: ComponentFixture<ElectionSelectexecutiveMemberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ElectionSelectexecutiveMemberComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ElectionSelectexecutiveMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
