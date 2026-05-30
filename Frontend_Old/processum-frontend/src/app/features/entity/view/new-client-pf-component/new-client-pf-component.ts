import { Component, inject } from '@angular/core';
import { CommonModule, Location, NgClass } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule } from "lucide-angular";
import { MessageModule } from 'primeng/message';
import { InputMaskModule } from 'primeng/inputmask';
import { InputTextModule } from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import { EntityIndividualRequest } from '../../../../dto/entity-individual-request';
import { maskDataPtBr, unMask } from '../../../../shared/utils/masks/masks';
import { EntityService } from '../../services/entity-service';
import { Router, ActivatedRoute } from '@angular/router';




@Component({
  selector: 'app-new-client-pf-component',
  imports: [LucideAngularModule, NgClass, MessageModule, InputMaskModule, InputTextModule, DatePickerModule, CommonModule, ReactiveFormsModule],
  templateUrl: './new-client-pf-component.html',
  styleUrl: './new-client-pf-component.css',
})
export class NewClientPfComponent {
  private formBuilder = inject(FormBuilder);
  private location = inject(Location);
  private entityService = inject(EntityService);
  private route = inject(Router);
  private activatedRoute = inject(ActivatedRoute);

  formSubmitted: boolean = false;
  isEdit = false;
  entityId?: string;
  clientForm: FormGroup;


  constructor() {
      this.clientForm = this.formBuilder.group({
        name: ['', [Validators.required, Validators.maxLength(255)]],
        cpf: ['', [Validators.required, Validators.maxLength(14)]],
        rg: ['', [Validators.maxLength(9)]],
        email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
        mobile: ['', [Validators.required, Validators.maxLength(15)]],
        phone: ['', [Validators.maxLength(14)]],
        birthDate: [null, Validators.required],
        address: ['', Validators.maxLength(255)],
        cep: [''],
        houseNumber: [''],
        complement: [''],
        city: [''],
        district: [''],
        uf: ['', [Validators.pattern(/^[A-Z]{2}$/)]],
      });
  }

  ngOnInit() {
      this.entityId = this.activatedRoute.snapshot.paramMap.get('id_public_entity') ?? undefined;

      if (this.entityId) {
        this.isEdit = true;
        this.loadClient();
      }
  }

  goBack() {
      this.location.back();
  }

  isInvalid(field: string) {
    const control = this.clientForm.get(field);
    return control && control.invalid && (control.dirty || control.touched);
  }

   onSubmit() {
            this.formSubmitted = true;
            if (this.clientForm.valid) {
              const formValues = this.clientForm.value;

              const entity: EntityIndividualRequest = {
                name: formValues.name,
                cpf: unMask(formValues.cpf),
                rg: unMask(formValues.rg),
                email: formValues.email,
                mobile: unMask(formValues.mobile),
                phone: unMask(formValues.phone),
                birthDate: new Date(formValues.birthDate).toISOString().split('T')[0],
                address: formValues.address,
                cep: unMask(formValues.cep),
                houseNumber: formValues.houseNumber,
                complement: formValues.complement,
                city: formValues.city,
                district: formValues.district,
                uf: formValues.uf
              };


              if(this.isEdit) {
                this.entityService.updateEntityIndividual(this.entityId!, entity).subscribe({
                  next: () =>{
                    this.route.navigate(['/entidades']);
                  },
                  error: (err) => {
                    console.error('Erro ao atualizar entidade', err);
                  },
                });

                return;

              }

              this.entityService.createEntityIndividual(entity).subscribe({
                next: () =>{
                  this.route.navigate(['/entidades']);
                },
                error: (err) => {
                  console.error('Erro ao criar entidade', err);
                },
              })
                // this.clientForm.reset();
                this.formSubmitted = false;
            }
    }

    loadClient() {

      this.entityService.getById(this.entityId!)
            .subscribe(pf => {
              console.log('Entidade carregada: ', pf);
              this.clientForm.patchValue({
                name: pf?.entityIndividual?.name,
                cpf: pf?.entityIndividual?.cpf,
                rg: pf?.entityIndividual?.rg,
                email: pf?.entityIndividual?.email,
                mobile: pf?.entityIndividual?.mobile,
                phone: pf?.entityIndividual?.phone,
                birthDate: maskDataPtBr(pf?.entityIndividual?.birthDate),
                address: pf?.entityIndividual?.address,
                cep: pf?.entityIndividual?.cep,
                houseNumber: pf?.entityIndividual?.houseNumber,
                complement: pf?.entityIndividual?.complement,
                city: pf?.entityIndividual?.city,
                district: pf?.entityIndividual?.district,
                uf: pf?.entityIndividual?.uf
              });

            });

    }

    deleteClient() {
      if(this.entityId) {
        debugger
        this.entityService.delete(this.entityId).subscribe({
          next: () => {
            this.route.navigate(['/entidades']);
          },
          error: (err) => {
            console.error('Erro ao deletar entidade', err);
          },
        });
      }
    }

    onUfInput(event: any) {
      const value = event.target.value.toUpperCase();
      this.clientForm.get('uf')?.setValue(value, { emitEvent: false });
    }

}
