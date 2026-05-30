import { ChangeDetectorRef, Component, inject, ViewChild } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputMaskModule } from 'primeng/inputmask';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { MultiSelectModule } from 'primeng/multiselect';
import { DatePickerModule } from 'primeng/datepicker';
import { MessageModule } from 'primeng/message';
import { TextareaModule } from 'primeng/textarea';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProcessService } from '../../services/process-service';
import { EntityService } from '../../../entity/services/entity-service';
import { Save, CircleArrowLeft } from 'lucide-angular/src/icons';
import { LucideAngularModule } from 'lucide-angular';
import { ProcessRequest } from '../../../../dto/process-request.model';
import { maskCpfCnpj, maskDataPtBr, unMask } from '../../../../shared/utils/masks/masks';
import { BehaviorSubject, filter, of } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SelectEntityComponent } from "../../../../shared/components/select-entity-component/select-entity-component";
import { EntityTableComponent } from "../../../entity/components/entity-table-component/entity-table-component";
import { ColDef } from 'ag-grid-community';
import { RemoveButtonCellRendererComponent } from '../../../../shared/components/remove-button-cell-renderer-component/remove-button-cell-renderer-component';
import { mapEntityToTable, toDate } from '../../../../shared/utils/helpers/helper';





const MODULES = [FloatLabelModule,
  InputMaskModule, TextareaModule,
  InputTextModule, SelectModule, DatePickerModule,
  CommonModule, ReactiveFormsModule, MessageModule,
  LucideAngularModule, MultiSelectModule
];


@Component({
  selector: 'app-new-process',
  imports: [...MODULES, SelectEntityComponent, EntityTableComponent],
  templateUrl: './new-process.html',
  styleUrl: './new-process.css',
})
export class NewProcess {

    private location = inject(Location);
    private formBuilder = inject(FormBuilder);
    private processService = inject(ProcessService);
    private entityService = inject(EntityService);
    private changeDetector = inject(ChangeDetectorRef);
    private route = inject(Router);
    private activatedRoute = inject(ActivatedRoute);
    @ViewChild('selectEntity') selectEntity!: SelectEntityComponent;

    private searchTimeout: any;
    protected readonly Save = Save;
    protected readonly CircleArrowLeft = CircleArrowLeft;
    private _acaoJudicialInicial: number | null = null;

    natures: any[] | undefined;
    actions: any[] | undefined;
    allClients: any[] = [];
    client: any[] = [];
    loadingClients = false;
    value: string | undefined;
    processForm: FormGroup;
    formSubmitted: boolean = false;
    isEdit = false;
    processId?: string;
    entitiesTable$ = new BehaviorSubject<any[]>([]);

   getProcessColDefs(): ColDef[] {
    const cols: ColDef[] = [
      {
        field: 'text',
        headerName: 'Nome',
        flex: 1
      },
      {
        field: 'cpfCnpj',
        headerName: 'CPF/CNPJ',
        flex: 1,
        valueGetter: ({ data }) => maskCpfCnpj(data?.cpfCnpj)
      },
      {
        field: 'email',
        headerName: 'Email',
        flex: 1
      },
      {
        field: 'mobile',
        headerName: 'Celular',
        flex: 1
      }
    ];

    // 🔥 só adiciona se NÃO for edição
    if (!this.isEdit) {
      cols.push({
        headerName: 'Excluir',
        width: 100,
        sortable: false,
        filter: false,
        cellRenderer: RemoveButtonCellRendererComponent,
        cellRendererParams: {
          onRemove: (data: any) => {
            const current = this.entitiesTable$.getValue();
            this.entitiesTable$.next(current.filter(e => e.id !== data.id));
          }
        }
      });
    }

    return cols;
  }

    constructor() {
      this.processForm = this.formBuilder.group({
                  numeroProcesso: ['', Validators.required],
                  dataAbertura: ['', Validators.required],
                  reclamante: [null],
                  reclamado: ['', Validators.required],
                  naturezaAcao: ['', Validators.required],
                  acaoJudicial: ['', Validators.required],
                  observacoes: [''] // opcional
              });
    }


    onSubmit() {
          this.formSubmitted = true;
          if (this.processForm.valid) {
            const formValues = this.processForm.value;

            const process: ProcessRequest = {
              processNumber: unMask(formValues.numeroProcesso),
              initialDate: new Date(formValues.dataAbertura).toISOString().split('T')[0],
              respondent: formValues.reclamado,
              description: formValues.observacoes,
              natureActionId: formValues.naturezaAcao,
              judicialActionId: formValues.acaoJudicial,
              EntityIds: formValues.reclamante
            };
            if (this.isEdit) {
              this.processService.updateProcess(this.processId!, process).subscribe({
                next: () => this.route.navigate(['/processos']),
                error: err => console.error('Erro ao atualizar processo', err)
              });
              return;
            }

            this.processService.createProcess(process).subscribe({
              next: () =>{
                this.route.navigate(['/processos']);
              },
              error: (err) => {
                console.error('Erro ao criar processo', err);
              },
            })

              this.formSubmitted = false;
              this.actions = [];
          }
      }
    goBack() {
      this.location.back();
    }
    isInvalid(controlName: string) {
          const control = this.processForm.get(controlName);
          return control?.invalid && (control.touched || this.formSubmitted);
    }

    ngOnInit(): void {
      this.processId = this.activatedRoute.snapshot.paramMap.get('id_public_process') ?? undefined;

      if (this.processId) {
        this.isEdit = true;
        this.loadProcess();
      }

      this.loadNatures();
      const natureAction = this.processForm.get('naturezaAcao');

      if (natureAction) {

        natureAction.valueChanges
        .pipe(filter(natureId => !!natureId))
        .subscribe(natureId => {
          this.processForm.get('acaoJudicial')?.reset();
          this.loadActions(natureId);
        });
      }


    }

    loadNatures() {
      this.processService.getNatures().subscribe(data => {
        this.natures = data;
      });
    }

    loadActions(natureId: number) {
      this.processService.getActions(natureId).subscribe(data => {
        this.actions = data;

        if (this._acaoJudicialInicial) {
          this.processForm.patchValue({
            acaoJudicial: this._acaoJudicialInicial
          });

          this._acaoJudicialInicial = null;
        }

      });
    }

    loadProcess() {
      this.processService.getById(this.processId!)
        .subscribe(proc => {
          console.log('Processo carregado: ', proc);
          console.log('Data formatada: ', maskDataPtBr(proc.initialDate ?? ''));

          this.processForm.patchValue({
            numeroProcesso: proc?.processNumber,
            dataAbertura: maskDataPtBr(proc.initialDate ?? ''),
            reclamado: proc?.respondent,
            naturezaAcao: proc?.natureAction.id,
            acaoJudicial: proc?.judicialAction.id,
            observacoes: proc?.description
          });

        const entitiesFormatted = (proc.entities || [])
          .map((e: any) => mapEntityToTable(e))
          .filter(Boolean);
        this.entitiesTable$.next(entitiesFormatted)

          this._acaoJudicialInicial = proc?.judicialAction.id;

        });
    }

    deleteProcess() {
      if (this.processId) {
        this.processService.deleteProcess(this.processId).subscribe({
          next: () => this.route.navigate(['/processos']),
          error: err => console.error('Erro ao deletar processo', err)
        });
      }
    }


  onEntitiesSelected(items: any[]) {
      const dataSourceCurrent = this.entitiesTable$.getValue();

      const updated = [
        ...dataSourceCurrent.filter(e => !items.some(i => i.id === e.id)),
        ...items
      ];

      this.entitiesTable$.next(updated);

      this.selectEntity.clear();
  }

  launchSelectedClient() {
    const selectedId = this.processForm.get('reclamante')?.value;
    if (!selectedId) return;

    // pega o objeto completo do select
    const selectedItem = this.selectEntity.allClients.find(c => c.id === selectedId);

    if (!selectedItem) return;

    const current = this.entitiesTable$.getValue();

    // evita duplicidade
    if (current.some(e => e.id === selectedItem.id)) {
      return;
    }

    this.entitiesTable$.next([...current, selectedItem]);

    // limpa select
    this.selectEntity.clear();
  }










}
