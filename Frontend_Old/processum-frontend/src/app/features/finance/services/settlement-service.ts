import { Injectable } from '@angular/core';
import { HttpBaseService } from '../../../shared/services/httpbase-service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../shared/environments/environment';
import { Observable } from 'rxjs';
import { Settlement } from '../../../models/settlement.model';
import { SettlementRequest } from '../../../dto/settlement-request.model';
import { SettlementFilterRequest } from '../../../dto/settlement-filter-request';


@Injectable({
  providedIn: 'root',
})
export class SettlementService extends HttpBaseService<Settlement, string> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/settlement`);
  }

  createSettlement(entity: SettlementRequest): Observable<Settlement> {
    return this.http.post<Settlement>(`${this.baseUrl}`, entity);
  }

  filterSettlements(filter: SettlementFilterRequest): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}?processNumber=${filter.processNumber}&status=${filter.status}`
    );
  }

  updateSettlement(idPublic: string, settlement: SettlementRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${idPublic}`, settlement);
  }

  deleteSettlement(idPublic: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${idPublic}`);
  }
}
