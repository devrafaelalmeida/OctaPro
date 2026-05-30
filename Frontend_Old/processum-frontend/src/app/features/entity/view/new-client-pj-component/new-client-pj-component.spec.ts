import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewClientPjComponent } from './new-client-pj-component';

describe('NewClientPjComponent', () => {
  let component: NewClientPjComponent;
  let fixture: ComponentFixture<NewClientPjComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewClientPjComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewClientPjComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
