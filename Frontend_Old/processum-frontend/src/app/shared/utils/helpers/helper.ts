import { FormGroup } from "@angular/forms";

export function resetForm(form: FormGroup, initialValues: Object) {
  form.reset(initialValues);
}

export function mapEntityToTable(entity: any) {
  if (entity.entityType === 'PF') {
    const pf = entity.entityIndividual;

    return {
      id: entity.idPublic,
      text: pf?.name,
      cpfCnpj: pf?.cpf,
      email: pf?.email,
      mobile: pf?.mobile,
      entityType: 'PF'
    };
  }

  if (entity.entityType === 'PJ') {
    const pj = entity.entityCompany;

    return {
      id: entity.idPublic,
      text: pj?.tradeName,
      cpfCnpj: pj?.cnpj,
      email: pj?.email,
      mobile: pj?.mobile,
      entityType: 'PJ'
    };
  }

  return null;
}

export function toDate(dateStr?: string): Date | null {
  if (!dateStr) return null;

  const [year, month, day] = dateStr.split('-').map(Number);

  return new Date(year, month - 1, day);
}
