import { TestBed } from '@angular/core/testing';

import { MaintblService } from './maintbl.service';

describe('MaintblService', () => {
  let service: MaintblService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MaintblService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
