import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminSocietyTranslationComponent } from './admin-society-translation.component';

describe('AdminSocietyTranslationComponent', () => {
  let component: AdminSocietyTranslationComponent;
  let fixture: ComponentFixture<AdminSocietyTranslationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdminSocietyTranslationComponent]
    });
    fixture = TestBed.createComponent(AdminSocietyTranslationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
