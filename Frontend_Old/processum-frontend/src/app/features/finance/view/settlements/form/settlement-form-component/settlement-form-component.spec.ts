import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SettlementFormComponent } from './settlement-form-component';

describe('SettlementFormComponent', () => {
  let component: SettlementFormComponent;
  let fixture: ComponentFixture<SettlementFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SettlementFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SettlementFormComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
