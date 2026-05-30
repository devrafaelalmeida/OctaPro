import { Component, Input } from '@angular/core';
import { ColDef } from 'ag-grid-community';
import { TableAgGridComponent } from '../../../../../shared/components/table-ag-grid/table-ag-grid';

@Component({
  selector: 'app-settlement-table',
  standalone: true,
  imports: [TableAgGridComponent],
  templateUrl: './settlement-table-component.html'
})
export class SettlementTableComponent {

  @Input({ required: true }) rowData$!: any;

  colDefs: ColDef[] = [
    {
      field: 'status',
      headerName: 'Status',
      flex: 1
    },
     {
      field: 'idPublic', //Por enquanto
      headerName: 'Acordo',
      flex: 1
    },
    {
      field: 'value',
      headerName: 'Documento',
      flex: 1
    },
    {
      field: 'value',
      headerName: 'Valor Parcela',
      flex: 1
    },
    {
      field: 'dueDate',
      headerName: 'Vencimento',
      flex: 1
    },
    {
      field: 'processNumber',
      headerName: 'Processo',
      flex: 1
    }
  ];

  defaultColDef: ColDef = {
    sortable: true,
    filter: true,
    resizable: true
  };
}
