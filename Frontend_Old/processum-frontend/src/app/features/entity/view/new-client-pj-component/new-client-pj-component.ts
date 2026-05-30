import { Component, inject } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideAngularModule } from "lucide-angular";
import { MessageModule } from 'primeng/message';
import { InputMaskModule } from 'primeng/inputmask';
import { InputTextModule } from 'primeng/inputtext';
import { EntityService } from '../../services/entity-service';
import { Router, ActivatedRoute } from '@angular/router';
import { unMask } from '../../../../shared/utils/masks/masks';

@Component({
  selector: 'app-new-client-pj-component',
  imports: [
    LucideAngularModule,
    MessageModule,
    InputMaskModule,
    InputTextModule,
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './new-client-pj-component.html',
  styleUrl: './new-client-pj-component.css',
})
export class NewClientPjComponent {

  private formBuilder = inject(FormBuilder);
  private location = inject(Location);
  private entityService = inject(EntityService);
  private route = inject(Router);
  private activatedRoute = inject(ActivatedRoute);

  formSubmitted: boolean = false;
  isEdit = false;
  entityId?: string;
  companyForm: FormGroup;

  constructor() {
    this.companyForm = this.formBuilder.group({
      cnpj: ['', [Validators.required, Validators.maxLength(18)]],
      corporateName: ['', [Validators.required, Validators.maxLength(200)]],
      tradeName: ['', [Validators.required, Validators.maxLength(200)]],
      corporateEmail: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      corporateMobile: ['', [Validators.maxLength(15)]],
      corporatePhone: ['', [Validators.maxLength(14)]],
      address: ['', [Validators.maxLength(255)]],
      cep: [''],
      houseNumber: [''],
      complement: ['', [Validators.maxLength(200)]],
      city: ['', [Validators.maxLength(255)]],
      district: ['', [Validators.maxLength(255)]],
      uf: ['', [Validators.pattern(/^[A-Z]{2}$/)]],
    });
  }

  ngOnInit() {
    this.entityId = this.activatedRoute.snapshot.paramMap.get('id_public_entity') ?? undefined;

    if (this.entityId) {
      this.isEdit = true;
      this.loadCompany();
    }
  }

  goBack() {
    this.location.back();
  }

  isInvalid(field: string) {
    const control = this.companyForm.get(field);
    return control && control.invalid && (control.dirty || control.touched);
  }

  onSubmit() {
    this.formSubmitted = true;

    if (this.companyForm.valid) {
      const formValues = this.companyForm.value;

      const entity = {
        cnpj: unMask(formValues.cnpj),
        corporateName: formValues.corporateName,
        tradeName: formValues.tradeName,
        corporateEmail: formValues.corporateEmail,
        corporateMobile: unMask(formValues.corporateMobile),
        corporatePhone: unMask(formValues.corporatePhone),
        address: formValues.address,
        cep: unMask(formValues.cep),
        houseNumber: formValues.houseNumber,
        complement: formValues.complement,
        city: formValues.city,
        district: formValues.district,
        uf: formValues.uf
      };

      if (this.isEdit) {
        this.entityService.updateEntityCompany(this.entityId!, entity).subscribe({
          next: () => {
            this.route.navigate(['/entidades']);
          },
          error: (err) => {
            console.error('Erro ao atualizar empresa', err);
          },
        });
        return;
      }

      this.entityService.createEntityCompany(entity).subscribe({
        next: () => {
          this.route.navigate(['/entidades']);
        },
        error: (err) => {
          console.error('Erro ao criar empresa', err);
        },
      });

      // this.companyForm.reset();
      this.formSubmitted = false;
    }
  }

  loadCompany() {
    this.entityService.getById(this.entityId!)
      .subscribe(pj => {

        this.companyForm.patchValue({
          cnpj: pj?.entityCompany?.cnpj,
          corporateName: pj?.entityCompany?.corporateName,
          tradeName: pj?.entityCompany?.tradeName,
          corporateEmail: pj?.entityCompany?.email,
          corporateMobile: pj?.entityCompany?.mobile,
          corporatePhone: pj?.entityCompany?.phone,
          address: pj?.entityCompany?.address,
          cep: pj?.entityCompany?.cep,
          houseNumber: pj?.entityCompany?.houseNumber,
          complement: pj?.entityCompany?.complement,
          city: pj?.entityCompany?.city,
          district: pj?.entityCompany?.district,
          uf: pj?.entityCompany?.uf
        });

      });
  }

  deleteCompany() {
    if (this.entityId) {
      this.entityService.delete(this.entityId).subscribe({
        next: () => {
          this.route.navigate(['/entidades']);
        },
        error: (err) => {
          console.error('Erro ao deletar empresa', err);
        },
      });
    }
  }

  onUfInput(event: any) {
    const value = event.target.value.toUpperCase();
    this.companyForm.get('uf')?.setValue(value, { emitEvent: false });
  }
}
