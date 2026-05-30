import { Routes } from '@angular/router';
import { ProcessComponent } from '../view/process/process';
import { NewProcess } from '../view/new-process/new-process';

export const PROCESS_ROUTES: Routes = [
  {
    path: 'processos',
    children: [
      { path: '', component: ProcessComponent },
      { path: 'novo', component: NewProcess },
      { path: 'editar/:id_public_process', component: NewProcess },


    ]
  }
];
