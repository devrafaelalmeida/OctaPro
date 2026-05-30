import { Injectable } from '@angular/core';
import { Process } from '../../../models/process.model';
import { HttpBaseService } from '../../../shared/services/httpbase-service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../shared/environments/environment';
import { Observable } from 'rxjs';
import { ProcessRequest } from '../../../dto/process-request.model';
import { ProcessFilterRequest } from '../../../dto/process-filter-request';

@Injectable({
  providedIn: 'root',
})
export class ProcessService extends HttpBaseService<Process, string> {

  constructor(http: HttpClient) {
    super(http, `${environment.apiUrl}/process`);
  }

  getNatures(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/natures`);
  }

  getActions(natureId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/actions/${natureId}`);
  }

  createProcess(entity: ProcessRequest): Observable<Process> {
    return this.http.post<Process>(`${this.baseUrl}`, entity);
  }

  filterProcesses(filter: ProcessFilterRequest): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}?processNumber=${filter.processNumber}&idPublicEntity=${filter.idPublicEntity}&status=${filter.statusId}&initialDate=${filter.initialDate}`);
  }

  updateProcess(idPublic: string, process: ProcessRequest): Observable<void> {
  return this.http.put<void>(`${this.baseUrl}/${idPublic}`, process);
  }

  deleteProcess(idPublic: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${idPublic}`);
  }


}
