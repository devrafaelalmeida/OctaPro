import { TestBed } from '@angular/core/testing';

import { ProcessModalService } from './process-modal-service';

describe('ProcessModalService', () => {
  let service: ProcessModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProcessModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
