import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserSocietyTypeComponent } from './user-society-type.component';

describe('UserSocietyTypeComponent', () => {
  let component: UserSocietyTypeComponent;
  let fixture: ComponentFixture<UserSocietyTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserSocietyTypeComponent]
    });
    fixture = TestBed.createComponent(UserSocietyTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
