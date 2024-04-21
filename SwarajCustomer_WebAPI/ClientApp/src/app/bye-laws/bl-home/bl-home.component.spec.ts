import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlHomeComponent } from './bl-home.component';

describe('BlHomeComponent', () => {
  let component: BlHomeComponent;
  let fixture: ComponentFixture<BlHomeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BlHomeComponent]
    });
    fixture = TestBed.createComponent(BlHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
