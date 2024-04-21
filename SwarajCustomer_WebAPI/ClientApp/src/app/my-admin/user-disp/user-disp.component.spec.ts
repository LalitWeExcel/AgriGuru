import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDispComponent } from './user-disp.component';

describe('UserDispComponent', () => {
  let component: UserDispComponent;
  let fixture: ComponentFixture<UserDispComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserDispComponent]
    });
    fixture = TestBed.createComponent(UserDispComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
