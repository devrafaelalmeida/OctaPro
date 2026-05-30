import { Component, inject } from '@angular/core';
import { ProcessService } from '../../services/process-service';
import { ProcessModalService } from '../../components/modals/details-process/services/process-modal-service';
import { DetailsProcess } from '../../components/modals/details-process/details-process';
import { ProcessTableComponent } from '../../components/process-table/process-table';
import { FilterProcessComponent } from "../../components/filter-process/filter-process-component";
import { ProcessFilterRequest } from '../../../../dto/process-filter-request';
import { unMask } from '../../../../shared/utils/masks/masks';
import { map, Observable } from 'rxjs';
import { Process } from '../../../../models/process.model';
import { ButtonNewRegisterComponent } from "../../../../shared/components/button-new-register-component/button-new-register-component";


@Component({
  selector: 'app-process',
  standalone: true,
  imports: [DetailsProcess, ProcessTableComponent, FilterProcessComponent, ButtonNewRegisterComponent],
  templateUrl: './process.html',
  styleUrl: './process.css',
})
export class ProcessComponent {

  private processService = inject(ProcessService);
  private detailsProcess = inject(ProcessModalService);
  private processFiltrerRequest!: ProcessFilterRequest;
  // processes$!: Observable<Process[]>;

  processes$ = this.processService.getAll();

  onView(row: any) {
    this.detailsProcess.open(row);
  }

  onEdit(row: any) {
  }

  onDelete(row: any) {
  }

  onFilter(filter: any) {
      this.processFiltrerRequest = {
        processNumber: filter.processNumber ? unMask(filter.processNumber) : "",
        idPublicEntity: filter.idPublicEntity ? filter.idPublicEntity : "",
        statusId: filter.status ? filter.status : "",
        initialDate: filter.initialDate ? filter.initialDate : ""
      };
      console.log(this.processFiltrerRequest);
      debugger
      this.processes$ = this.processService.filterProcesses(this.processFiltrerRequest);




      // this.processes$ = this.processService.filterProcesses(this.processFiltrerRequest).pipe(
      //   map((process: any[]) =>
      //     process
      //       .filter(e => {
      //         if (filter.cpf) {
      //           return e.entityIndividual?.cpf?.includes(filter.cpf);
      //         }
      //         return true;
      //       })
      //       .map(e => {
      //         return {
      //            idPublic: e.idPublic,
      //             userId: e.user,
      //             judicialProcessId: e.processNumber,
      //             statusPaymentId: e.isArchived ? 1 : 0,
      //             note: e.description,
      //             entities: e.entities,
      //             createdAt: e.createdAt,
      //             updatedAt: e.createdAt
      //         };
      //       })
      //   )
      // );
    }
}
