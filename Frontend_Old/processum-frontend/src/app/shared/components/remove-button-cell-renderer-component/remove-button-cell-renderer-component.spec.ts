import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveButtonCellRendererComponent } from './remove-button-cell-renderer-component';

describe('RemoveButtonCellRendererComponent', () => {
  let component: RemoveButtonCellRendererComponent;
  let fixture: ComponentFixture<RemoveButtonCellRendererComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoveButtonCellRendererComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RemoveButtonCellRendererComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
