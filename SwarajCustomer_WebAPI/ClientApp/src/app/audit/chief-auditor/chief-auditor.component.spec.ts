import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChiefAuditorComponent } from './chief-auditor.component';

describe('ChiefAuditorComponent', () => {
  let component: ChiefAuditorComponent;
  let fixture: ComponentFixture<ChiefAuditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChiefAuditorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ChiefAuditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
