import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ColDef } from 'ag-grid-community';

import { ButtonActionGrid } from '../button-action-grid/button-action-grid';

import { maskDataPtBr, maskProcessNumber } from '../../../../shared/utils/masks/masks';
import { Eye, Pencil, Trash2 } from 'lucide-angular/src/icons';
import { TableAgGridComponent } from '../../../../shared/components/table-ag-grid/table-ag-grid';
import { Router } from '@angular/router';

@Component({
  selector: 'app-process-table',
  standalone: true,
  imports: [
    TableAgGridComponent
  ],
  templateUrl: './process-table.html',
  styleUrl: './process-table.css',
})
export class ProcessTableComponent {

  @Input({ required: true }) rowData$!: any;

  @Output() view = new EventEmitter<any>();
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();

  private router = inject(Router);


  onRowClicked(event: any) {
    const id = event.data.idPublic;

    if (id) {
      this.router.navigate(['processos/editar/', id]);
    }
  }

  colDefs: ColDef[] = [
    {
      field: 'processNumber',
      headerName: 'Nº Processo',
      flex: 1,
      minWidth: 200,
      valueFormatter: p => maskProcessNumber(p.value)
    },
    {
      headerName: 'Cliente',
      flex: 1,
      minWidth: 250,
      valueGetter: p =>
        p.data.entities[0]?.entityType === 'PF'
          ? p.data.entities[0]?.entityIndividual?.name
          : p.data.entities[0]?.entityCompany?.tradeName
    },
    {
      headerName: 'Natureza',
      valueGetter: p => p.data?.natureAction?.text ?? ''
    },
    {
      field: 'isArchived',
      headerName: 'Status',
      valueGetter: p => p.data?.isArchived ? 'Arquivado' : 'Em Andamento'
    },
    {
      field: 'initialDate',
      headerName: 'Data Inicial',
      valueFormatter: p => maskDataPtBr(p.value)
    },
  ];

  defaultColDef: ColDef = {
    filter: true,
    sortable: true,
    resizable: true,
  };

  gridOptions = {
    enableCellTextSelection: true,
    ensureDomOrder: true,
    domLayout: 'normal'
  };
}
