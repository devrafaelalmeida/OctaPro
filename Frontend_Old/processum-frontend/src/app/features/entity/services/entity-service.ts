import { Injectable } from '@angular/core';
import { HttpBaseService } from '../../../shared/services/httpbase-service';
import { ProcessEntity } from '../../../models/process-entity.model';
import { environment } from '../../../shared/environments/environment';
import { HttpClient } from '@angular/common/http';
import { EntityIndividualRequest } from '../../../dto/entity-individual-request';
import { Observable } from 'rxjs';
import { Entity } from '../../../models/entity.model';
import { EntityCompanyRequest } from '../../../dto/entity-company-request';
import { FilterEntitiesRequest } from '../../../dto/filter-entities-request';


@Injectable({
  providedIn: 'root',
})
export class EntityService extends HttpBaseService<Entity, string> {
    constructor(http: HttpClient) {
      super(http, `${environment.apiUrl}/entity`);
    }

    filterClients(filter: FilterEntitiesRequest): Observable<any[]> {
      return this.http.get<any[]>(`${this.baseUrl}?IdPublicEntity=${filter.idPublicEntity}&status=${filter.statusId}&cpfCnpj=${filter.cpf_cnpj}`);
    }


    searchClients(client: string) {
      return this.http.get<any[]>(`${this.baseUrl}/clients?name=${client}`);
    }

    createEntityIndividual(entity: EntityIndividualRequest): Observable<void> {
      return this.http.post<void>(`${this.baseUrl}/pf`, entity);
    }

    updateEntityIndividual(idPublic: string, entity: EntityIndividualRequest): Observable<void> {
      return this.http.put<void>(`${this.baseUrl}/pf/${idPublic}`, entity);
    }

    createEntityCompany(entity: EntityCompanyRequest): Observable<void> {
      return this.http.post<void>(`${this.baseUrl}/pj`, entity);
    }

    updateEntityCompany(idPublic: string, entity: EntityCompanyRequest): Observable<void> {
      return this.http.put<void>(`${this.baseUrl}/pj/${idPublic}`, entity);
    }




}
