import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewClientPfComponent } from './new-client-pf-component';

describe('NewClientPfComponent', () => {
  let component: NewClientPfComponent;
  let fixture: ComponentFixture<NewClientPfComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewClientPfComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewClientPfComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
