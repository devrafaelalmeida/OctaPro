import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LucideAngularModule } from 'lucide-angular';
import { Funnel, Search, Eraser } from 'lucide-angular/src/icons';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputMaskModule } from 'primeng/inputmask';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectEntityComponent } from "../../../../shared/components/select-entity-component/select-entity-component";
import { maskDataUSA, unMask } from '../../../../shared/utils/masks/masks';
import { resetForm } from '../../../../shared/utils/helpers/helper';


interface Status {
    name: string;
    code: string;
}

const MODULES = [FloatLabelModule, InputMaskModule, FormsModule, LucideAngularModule, InputTextModule, SelectModule, DatePickerModule];


@Component({
  selector: 'app-filter-process-component',
  imports: [...MODULES, SelectEntityComponent, ReactiveFormsModule],
  templateUrl: './filter-process-component.html',
  styleUrl: './filter-process-component.css',
})
export class FilterProcessComponent {
    protected readonly Funnel = Funnel;
    protected readonly Search = Search;
    protected readonly Eraser = Eraser;
    @Output() filterValues = new EventEmitter<any>();


    status: Status[] = [
      { name: 'Em Andamento', code: '1' },
      { name: 'Arquivado', code: '2' },
    ];

    statusSelected: Status = this.status[0];

    filterProcessForm = new FormGroup({
      processNumber: new FormControl<string | null>(null),
      idPublicEntity: new FormControl<string | null>(null),
      status: new FormControl<any | null>(null),
      initialDate: new FormControl<string | null>(null)
    });

    applyFilter() {

        const form = this.filterProcessForm.value;

        const filterFormTrated = {
          ...form,
          processNumber: form.processNumber ? unMask(form.processNumber) : "",
          idPublicEntity: form.idPublicEntity ? form.idPublicEntity : "",
          status: form.status ? form.status.code : "",
          initialDate: form.initialDate ? maskDataUSA(form.initialDate) : ""
        };
        this.filterValues.emit(filterFormTrated);
      }

    resetFilter() {
      const initialValues = {
        processNumber: null,
      idPublicEntity: null,
      status: this.status[0],
      initialDate: null
      }
      resetForm(this.filterProcessForm, initialValues);
    }

}
