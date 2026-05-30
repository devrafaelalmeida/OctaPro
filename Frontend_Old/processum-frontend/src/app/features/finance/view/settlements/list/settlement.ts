import { Component, inject } from '@angular/core';

import { BehaviorSubject, Observable, of } from 'rxjs';
import { SettlementService } from '../../../services/settlement-service';
import { SettlementFilterComponent } from '../../../components/filter-settlement/filter-settlement-component/filter-settlement-component';
import { SettlementTableComponent } from '../../../components/settlement-table/settlement-table-component/settlement-table-component';
import { ButtonNewRegisterComponent } from "../../../../../shared/components/button-new-register-component/button-new-register-component";

@Component({
  selector: 'app-settlement',
  standalone: true,
  imports: [SettlementFilterComponent, SettlementTableComponent, ButtonNewRegisterComponent],
  templateUrl: './settlement.html',
  styleUrl: './settlement.css',
})
export class SettlementComponent {

  private settlementService = inject(SettlementService);

  settlements$ = new BehaviorSubject<any[]>([]);    // settlements$ = [];

  // onFilter(filter: any) {
  //   this.settlementService
  //     .filterSettlements(filter)
  //     .subscribe(data => this.settlements$.next(data));
  // }
}
