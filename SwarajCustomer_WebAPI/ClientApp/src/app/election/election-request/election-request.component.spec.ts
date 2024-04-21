import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectionRequestComponent } from './election-request.component';

describe('ElectionRequestComponent', () => {
  let component: ElectionRequestComponent;
  let fixture: ComponentFixture<ElectionRequestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ElectionRequestComponent]
    });
    fixture = TestBed.createComponent(ElectionRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
