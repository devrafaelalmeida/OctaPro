import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessTableComponent } from './process-table';

describe('ProcessTable', () => {
  let component: ProcessTableComponent;
  let fixture: ComponentFixture<ProcessTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProcessTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProcessTableComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
