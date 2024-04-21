import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MSharedComponent } from './m-shared.component';

describe('MSharedComponent', () => {
  let component: MSharedComponent;
  let fixture: ComponentFixture<MSharedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MSharedComponent]
    });
    fixture = TestBed.createComponent(MSharedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
