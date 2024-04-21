import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleModuleComponent } from './role-module.component';

describe('RoleModuleComponent', () => {
  let component: RoleModuleComponent;
  let fixture: ComponentFixture<RoleModuleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RoleModuleComponent]
    });
    fixture = TestBed.createComponent(RoleModuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
