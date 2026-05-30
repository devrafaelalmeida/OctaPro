import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { FloatLabelModule } from 'primeng/floatlabel';
import { LucideAngularModule } from "lucide-angular";
import { InputMask } from "primeng/inputmask";

@Component({
  selector: 'app-filter-settlement',
  standalone: true,
  imports: [ReactiveFormsModule, InputTextModule, SelectModule, FloatLabelModule, LucideAngularModule, InputMask],
  templateUrl: './filter-settlement-component.html'
})
export class SettlementFilterComponent {

  @Output() filterValues = new EventEmitter<any>();

  statusPayment = [
    { name: 'Pago', code: '1' },
    { name: 'Pendente', code: '2' }
  ];

  filterSettlementForm = new FormGroup({
    processNumber: new FormControl<string | null>(null),
    status: new FormControl<any | null>(null)
  });

  applyFilter() {
    const form = this.filterSettlementForm.value;

    this.filterValues.emit({
      processNumber: form.processNumber ?? '',
      status: form.status?.code ?? ''
    });
  }

  resetFilter() {
    this.filterSettlementForm.reset();
  }
}
