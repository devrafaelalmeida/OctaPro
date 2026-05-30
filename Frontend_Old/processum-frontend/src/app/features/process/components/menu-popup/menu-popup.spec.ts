import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActionsMenuPopupComponent } from './menu-popup';

describe('ActionsMenuPopupComponent', () => {
  let component: ActionsMenuPopupComponent;
  let fixture: ComponentFixture<ActionsMenuPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActionsMenuPopupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ActionsMenuPopupComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
