import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community';
import { AsyncPipe } from '@angular/common';

import { AG_GRID_LOCALE_PT_BR } from '../../../shared/ag-grid/ag-grid-locale-pt';
import { AG_GRID_THEME_CUSTOM } from '../../ag-grid/theme-custom';

@Component({
  selector: 'app-table-ag-grid',
  standalone: true,
  imports: [AgGridAngular, AsyncPipe],
  templateUrl: './table-ag-grid.html',
  styleUrl: './table-ag-grid.css',
})
export class TableAgGridComponent {

  @Input({ required: true }) rowData$!: any;
  @Input({ required: true }) colDefs!: ColDef[];

  @Input() defaultColDef: ColDef = {
    filter: true,
    sortable: true,
    resizable: true,
  };

  @Input() gridOptions: any = {};
  @Output() rowClicked = new EventEmitter<any>();

  onRowClicked(event: any) {
    this.rowClicked.emit(event);
  }

  localeText = AG_GRID_LOCALE_PT_BR;
  theme = AG_GRID_THEME_CUSTOM

}
