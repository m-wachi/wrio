import { TestBed } from '@angular/core/testing';

import { MaintblService } from './maintbl.service';

describe('MaintblService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MaintblService = TestBed.get(MaintblService);
    expect(service).toBeTruthy();
  });
});
