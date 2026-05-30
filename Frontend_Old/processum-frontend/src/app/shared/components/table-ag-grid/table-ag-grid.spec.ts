import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableAgGrid } from './table-ag-grid';

describe('TableAgGrid', () => {
  let component: TableAgGrid;
  let fixture: ComponentFixture<TableAgGrid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableAgGrid]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TableAgGrid);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
