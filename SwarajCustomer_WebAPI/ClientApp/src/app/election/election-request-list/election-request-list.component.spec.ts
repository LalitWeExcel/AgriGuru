import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ElectionRequestListComponent } from './election-request-list.component';

describe('ElectionRequestListComponent', () => {
  let component: ElectionRequestListComponent;
  let fixture: ComponentFixture<ElectionRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ElectionRequestListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ElectionRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
