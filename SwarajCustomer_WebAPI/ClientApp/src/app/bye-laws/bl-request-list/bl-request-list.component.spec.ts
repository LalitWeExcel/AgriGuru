import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlRequestListComponent } from './bl-request-list.component';

describe('BlRequestListComponent', () => {
  let component: BlRequestListComponent;
  let fixture: ComponentFixture<BlRequestListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BlRequestListComponent]
    });
    fixture = TestBed.createComponent(BlRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
