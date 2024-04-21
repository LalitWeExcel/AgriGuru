import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MGrivanceComponent } from './m-grivance.component';

describe('MGrivanceComponent', () => {
  let component: MGrivanceComponent;
  let fixture: ComponentFixture<MGrivanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MGrivanceComponent]
    });
    fixture = TestBed.createComponent(MGrivanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
