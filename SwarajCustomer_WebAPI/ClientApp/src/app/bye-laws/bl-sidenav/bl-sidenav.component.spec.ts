import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlSidenavComponent } from './bl-sidenav.component';

describe('BlSidenavComponent', () => {
  let component: BlSidenavComponent;
  let fixture: ComponentFixture<BlSidenavComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BlSidenavComponent]
    });
    fixture = TestBed.createComponent(BlSidenavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
