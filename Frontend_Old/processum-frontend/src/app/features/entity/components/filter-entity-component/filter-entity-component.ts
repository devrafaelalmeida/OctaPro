import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SelectEntityComponent } from "../../../../shared/components/select-entity-component/select-entity-component";
import { LucideAngularModule } from 'lucide-angular';
import { Funnel, Search, Eraser } from 'lucide-angular/src/icons';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputMaskModule } from 'primeng/inputmask';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { unMask } from '../../../../shared/utils/masks/masks';
import { resetForm } from '../../../../shared/utils/helpers/helper';

const MODULES = [FloatLabelModule, InputMaskModule, FormsModule, LucideAngularModule, InputTextModule, SelectModule, DatePickerModule,
  ReactiveFormsModule,

  SelectEntityComponent
];

interface StatusEntity {
    name: string;
    code: string;
}

@Component({
  selector: 'app-filter-entity-component',
  imports: [...MODULES],
  templateUrl: './filter-entity-component.html',
  styleUrl: './filter-entity-component.css',
})
export class FilterEntityComponent {

  protected readonly Funnel = Funnel;
  protected readonly Search = Search;
  protected readonly Eraser = Eraser;
  @Output() filterValues = new EventEmitter<any>();

  statusEntity: StatusEntity[] = [
      { name: 'Ativo', code: '1' },
      { name: 'Inativo', code: '2' },
    ];
  filterForm = new FormGroup({
    reclamante: new FormControl(null),
    status: new FormControl(this.statusEntity[0]),
    cpf_cnpj: new FormControl(null)
  });

  applyFilter() {

    const form = this.filterForm.value;

    const filterFormTrated = {
      ...form,
      cpf_cnpj: form.cpf_cnpj ? unMask(form.cpf_cnpj) : null,
      status: form.status ? form.status.code : null
    };
    this.filterValues.emit(filterFormTrated);
  }

  resetFilter() {
    const initialValues = {
      reclamante: null,
      status: this.statusEntity[0],
      cpf_cnpj: null
    };
    resetForm(this.filterForm, initialValues);
  }

}
