import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterProcessComponent } from './filter-process-component';

describe('FilterProcessComponent', () => {
  let component: FilterProcessComponent;
  let fixture: ComponentFixture<FilterProcessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterProcessComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FilterProcessComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
