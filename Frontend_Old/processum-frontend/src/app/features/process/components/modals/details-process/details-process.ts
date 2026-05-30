import { Component, inject } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { ProcessModalService } from './services/process-modal-service';
import { AsyncPipe, CommonModule, JsonPipe } from '@angular/common';
import { maskDataPtBr } from '../../../../../shared/utils/masks/masks';
import { map } from 'rxjs';


@Component({
  selector: 'app-details-process',
  imports: [Dialog, AsyncPipe, CommonModule],
  templateUrl: './details-process.html',
  styleUrl: './details-process.css',
})
export class DetailsProcess {

  private detailsProcess = inject(ProcessModalService)

    visible$ = this.detailsProcess.visible$;
    process$ = this.detailsProcess.process$.pipe(
      map(process => {
        if (!process) return null;
        console.log(process);
        return {
          ...process,
          initialDate: maskDataPtBr(process.initialDate),
          isArchived: process.isArchived ? 'Arquivado' : 'Em Andamento'
        };
      })
    );

    close() {
    this.detailsProcess.close();
  }
}
