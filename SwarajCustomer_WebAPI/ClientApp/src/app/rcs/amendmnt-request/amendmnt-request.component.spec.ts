import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmendmntRequestComponent } from './amendmnt-request.component';

describe('AmendmntRequestComponent', () => {
  let component: AmendmntRequestComponent;
  let fixture: ComponentFixture<AmendmntRequestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AmendmntRequestComponent]
    });
    fixture = TestBed.createComponent(AmendmntRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
