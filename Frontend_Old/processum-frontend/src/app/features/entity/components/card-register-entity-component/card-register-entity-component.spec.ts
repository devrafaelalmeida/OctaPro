import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardRegisterEntityComponent } from './card-register-entity-component';

describe('CardRegisterEntityComponent', () => {
  let component: CardRegisterEntityComponent;
  let fixture: ComponentFixture<CardRegisterEntityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardRegisterEntityComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardRegisterEntityComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
