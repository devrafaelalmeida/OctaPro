import { Component, ChangeDetectorRef, forwardRef, Input, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MultiSelectModule } from 'primeng/multiselect';
import { EntityService } from '../../../features/entity/services/entity-service';
import { SelectModule } from 'primeng/select';
import { FloatLabelModule } from "primeng/floatlabel";


@Component({
  selector: 'app-select-entity-component',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MultiSelectModule,
    SelectModule,
    FloatLabelModule
],
  templateUrl: './select-entity-component.html',
  styleUrl: './select-entity-component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectEntityComponent),
      multi: true
    }
  ]
})
export class SelectEntityComponent implements ControlValueAccessor {
  @Input() multiple: boolean = true;
  @Input() placeholder: string = "";
  @Input() label: string = '';
  @Output() selectedItemsChange = new EventEmitter<any[]>();


  clients: any[] = [];
  allClients: any[] = [];

  loadingClients = false;

  private searchTimeout: any;

  value: any = [];

  onChange = (value: any) => {};
  onTouched = () => {};

  constructor(
    private entityService: EntityService,
    private cd: ChangeDetectorRef
  ) {}

  writeValue(value: any): void {
    if (this.multiple) {
      this.value = value ?? [];
    } else {
      this.value = value ?? null;
    }

    this.cd.detectChanges();
  }

  clear() {
    this.value = this.multiple ? [] : null;

    this.onTouched();
    this.onChange(this.value);

    this.clients = [];

    this.cd.detectChanges();
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  searchClients(event: any) {

    const query = event.filter;

    if (!query || query.length < 2) {
      if (this.searchTimeout) clearTimeout(this.searchTimeout);
      return;
    }

    if (this.searchTimeout) clearTimeout(this.searchTimeout);

    this.searchTimeout = setTimeout(() => {

      this.loadingClients = true;

      this.entityService.searchClients(query)
        .subscribe({
          next: (data) => {

            const selectedIds = this.value || [];

            this.allClients = [
              ...this.allClients,
              ...data.filter(d => !this.allClients.some(a => a.id === d.id))
            ];

            const selectedItems = this.allClients.filter(c =>
              selectedIds.includes(c.id)
            );

            this.clients = [
              ...selectedItems.map(i => ({ ...i })),
              ...data
                .filter(d => !selectedIds.includes(d.id))
                .map(i => ({ ...i }))
            ];

            this.loadingClients = false;
            this.cd.detectChanges();
          },
          error: () => {
            this.loadingClients = false;
          }
        });

    }, 500);
  }

  onSelectChange(event: any) {
  if (this.multiple) {
    this.value = event.value ?? [];
  } else {
    this.value = event.value ?? null;
  }

  this.onChange(this.value);

  const selectedItems = this.allClients.filter(c =>
    this.multiple
      ? this.value?.includes(c.id)
      : c.id === this.value
  );

  this.selectedItemsChange.emit(selectedItems);
}


}
