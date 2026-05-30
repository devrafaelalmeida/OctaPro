import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonNewRegisterComponent } from './button-new-register-component';

describe('ButtonNewRegisterComponent', () => {
  let component: ButtonNewRegisterComponent;
  let fixture: ComponentFixture<ButtonNewRegisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ButtonNewRegisterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ButtonNewRegisterComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
