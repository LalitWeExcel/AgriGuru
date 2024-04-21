import { TestBed } from '@angular/core/testing';

import { ElectionServiceService } from './election-service.service';

describe('ElectionServiceService', () => {
  let service: ElectionServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ElectionServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
