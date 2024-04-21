import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CooperativeDetailsComponent } from './cooperative-details.component';

describe('CooperativeDetailsComponent', () => {
  let component: CooperativeDetailsComponent;
  let fixture: ComponentFixture<CooperativeDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CooperativeDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CooperativeDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
