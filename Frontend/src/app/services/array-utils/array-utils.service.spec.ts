import { TestBed } from '@angular/core/testing';

import { ArrayUtilsService } from './array-utils.service';

describe('ArrayUtilsService', () => {
  let service: ArrayUtilsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ArrayUtilsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
