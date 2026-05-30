import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewClientButtonComponent } from './new-client-button-component';

describe('NewClientButtonComponent', () => {
  let component: NewClientButtonComponent;
  let fixture: ComponentFixture<NewClientButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewClientButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewClientButtonComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
