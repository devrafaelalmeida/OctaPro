import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewProcessButton } from './new-process-button';

describe('NewProcessButton', () => {
  let component: NewProcessButton;
  let fixture: ComponentFixture<NewProcessButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewProcessButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewProcessButton);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
