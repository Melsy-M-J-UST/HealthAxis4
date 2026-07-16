import { TestBed } from '@angular/core/testing';

import { HealthRecordService } from './health-record';

describe('HealthRecord', () =>
{
  let service: HealthRecordService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HealthRecordService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
