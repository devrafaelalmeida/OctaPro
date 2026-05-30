import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ColDef } from 'ag-grid-community';


import { Eye, Pencil, Trash2 } from 'lucide-angular/src/icons';
import { TableAgGridComponent } from '../../../../shared/components/table-ag-grid/table-ag-grid';
import { ButtonActionGrid } from '../../../process/components/button-action-grid/button-action-grid';
import { Router } from '@angular/router';
import { maskCpfCnpj } from '../../../../shared/utils/masks/masks';
import { startWith } from 'rxjs';

@Component({
  selector: 'app-entity-table',
  standalone: true,
  imports: [
    TableAgGridComponent
  ],
  templateUrl: './entity-table-component.html',
  styleUrl: './entity-table-component.css',
})
export class EntityTableComponent {

  @Input({ required: true }) rowData$!: any;
  @Input() enableNavigation = true;
  @Input() customColDefs?: ColDef[];

  get colDefs(): ColDef[] {
    return this.customColDefs ?? this.defaultColDefs;
  }

  @Output() view = new EventEmitter<any>();
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();

  private router = inject(Router);

  onRowClicked(event: any) {
    if (!this.enableNavigation) {
    this.view.emit(event.data);
    return;
  }
    const id = event.data.idPublic;

    if (event.data.entityType === 'PF') {
      this.router.navigate(['entidades/editar/pf', id]);
    }else {
      this.router.navigate(['entidades/editar/pj', id]);
    }
  }


  // colDefs: ColDef[] = [
  //   {
  //     field: 'name',
  //     headerName: 'Nome',
  //     flex: 1,
  //     minWidth: 200
  //   },
  //   {
  //     field: 'corporateName',
  //     headerName: 'Razão Social',
  //     flex: 1,
  //     minWidth: 250
  //   },
  //   {
  //     headerName: 'Documento',
  //     flex: 1,
  //     valueGetter: ({ data }) => {

  //       if (!data) return '';

  //       return data.entityType === 'PF'
  //         ? maskCpfCnpj(data?.cpf)
  //         : maskCpfCnpj(data?.cnpj);
  //     }
  //   },
  //   {
  //     field: 'entityType',
  //     headerName: 'Pessoa',
  //     flex: 1,
  //     valueGetter: ({ data }) => {

  //       if (!data) return '';

  //       return data.entityType === 'PF'
  //         ? 'Física'
  //         : 'Jurídica';
  //     }
  //   },
  //   {
  //     field: 'email',
  //     headerName: 'Email',
  //     flex: 1
  //   },
  // ];

  private defaultColDefs: ColDef[] = [
    {
      field: 'name',
      headerName: 'Nome',
      flex: 1,
      minWidth: 200
    },
    {
      field: 'corporateName',
      headerName: 'Razão Social',
      flex: 1,
      minWidth: 250
    },
    {
      headerName: 'Documento',
      flex: 1,
      valueGetter: ({ data }) => {
        if (!data) return '';
        return data.entityType === 'PF'
          ? maskCpfCnpj(data?.cpf)
          : maskCpfCnpj(data?.cnpj);
      }
    },
    {
      field: 'entityType',
      headerName: 'Pessoa',
      flex: 1,
      valueGetter: ({ data }) => {
        if (!data) return '';
        return data.entityType === 'PF' ? 'Física' : 'Jurídica';
      }
    },
    {
      field: 'email',
      headerName: 'Email',
      flex: 1
    }
  ];

  defaultColDef: ColDef = {
    filter: true,
    sortable: true,
    resizable: true,
  };

  gridOptions = {
    // popupParent: document.body,
    // suppressRowTransform: true
    enableCellTextSelection: true,
    ensureDomOrder: true,
    domLayout: 'normal'

  };

  ngOnInit() {
    this.rowData$.pipe(startWith([])).subscribe(); // garante emissão inicial
  }

}
