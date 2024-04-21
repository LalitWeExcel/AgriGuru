import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MMemberbacktrackComponent } from './m-memberbacktrack.component';

describe('MMemberbacktrackComponent', () => {
  let component: MMemberbacktrackComponent;
  let fixture: ComponentFixture<MMemberbacktrackComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MMemberbacktrackComponent]
    });
    fixture = TestBed.createComponent(MMemberbacktrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
