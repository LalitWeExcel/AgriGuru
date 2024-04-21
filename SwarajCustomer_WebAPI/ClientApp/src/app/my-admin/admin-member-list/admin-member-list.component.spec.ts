import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMemberListComponent } from './admin-member-list.component';

describe('AdminMemberListComponent', () => {
  let component: AdminMemberListComponent;
  let fixture: ComponentFixture<AdminMemberListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminMemberListComponent]
    });
    fixture = TestBed.createComponent(AdminMemberListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
