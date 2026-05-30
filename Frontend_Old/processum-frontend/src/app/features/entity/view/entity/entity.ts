import { Component, inject } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { EntityTableComponent } from '../../components/entity-table-component/entity-table-component';

import { User, Building } from 'lucide-angular/src/icons';
import { EntityService } from '../../services/entity-service';
import { FilterEntityComponent } from "../../components/filter-entity-component/filter-entity-component";
import { FilterEntitiesRequest } from '../../../../dto/filter-entities-request';
import { unMask } from '../../../../shared/utils/masks/masks';
import { NewClientButtonComponent } from "../../components/new-client-button-component/new-client-button-component";

@Component({
  selector: 'app-entity',
  standalone: true,
  imports: [
    EntityTableComponent,
    FilterEntityComponent,
    NewClientButtonComponent
],
  templateUrl: './entity.html',
  styleUrl: './entity.css',
})
export class EntityComponent {

  public readonly User = User;
  public readonly Building = Building;
  private entityService = inject(EntityService);
  private filterRequest!: FilterEntitiesRequest;

  entities$ = this.entityService.getAll().pipe(
    map((entities: any[]) => entities.map(e => {
      const isPF = e.entityType === 'PF';

      return {
        idPublic: e.idPublic,
        entityType: e.entityType,

        name: isPF ? e.entityIndividual?.name : e.entityCompany?.tradeName,
        corporateName: isPF ? null : e.entityCompany?.corporateName,

        cpf: isPF ? e.entityIndividual?.cpf : null,
        cnpj: isPF ? null : e.entityCompany?.cnpj,

        email: isPF ? e.entityIndividual?.email : e.entityCompany?.email
      };
    }))
  );

  onFilter(filter: any) {
    this.filterRequest = {
      idPublicEntity: filter.reclamante ? filter.reclamante : "",
      statusId: filter.status ? filter.status : "",
      cpf_cnpj: filter.cpf_cnpj  ? unMask(filter.cpf_cnpj) : ""
    };
    // exemplo:
    this.entities$ = this.entityService.filterClients(this.filterRequest).pipe(
      map((entities: any[]) =>
        entities
          .filter(e => {
            if (filter.cpf) {
              return e.entityIndividual?.cpf?.includes(filter.cpf);
            }
            return true;
          })
          .map(e => {
            const isPF = e.entityType === 'PF';

            return {
              idPublic: e.idPublic,
              entityType: e.entityType,
              name: isPF ? e.entityIndividual?.name : e.entityCompany?.tradeName,
              corporateName: isPF ? null : e.entityCompany?.corporateName,
              cpf: isPF ? e.entityIndividual?.cpf : null,
              cnpj: isPF ? null : e.entityCompany?.cnpj,
              email: isPF ? e.entityIndividual?.email : e.entityCompany?.email
            };
          })
      )
    );
  }



}
