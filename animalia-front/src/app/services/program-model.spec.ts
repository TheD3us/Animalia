import { TestBed } from '@angular/core/testing';

import { ProgramModel } from './program-model';

describe('ProgramModel', () => {
  let service: ProgramModel;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProgramModel);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
