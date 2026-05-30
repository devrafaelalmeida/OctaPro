import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SettlementTableComponent } from './settlement-table-component';

describe('SettlementTableComponent', () => {
  let component: SettlementTableComponent;
  let fixture: ComponentFixture<SettlementTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SettlementTableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SettlementTableComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
