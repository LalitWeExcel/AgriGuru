import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttachmentDocumentsComponent } from './attachment-documents.component';

describe('AttachmentDocumentsComponent', () => {
  let component: AttachmentDocumentsComponent;
  let fixture: ComponentFixture<AttachmentDocumentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AttachmentDocumentsComponent]
    });
    fixture = TestBed.createComponent(AttachmentDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
