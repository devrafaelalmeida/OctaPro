import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SettlementFilterComponent } from './filter-settlement-component';

describe('SettlementFilterComponent', () => {
  let component: SettlementFilterComponent;
  let fixture: ComponentFixture<SettlementFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SettlementFilterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SettlementFilterComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
