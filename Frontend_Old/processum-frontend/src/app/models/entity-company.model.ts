export interface EntityCompany {
  idPublic: string;
  entityType?: string | null;

  cnpj?: string | null;
  corporateName?: string | null;
  tradeName?: string | null;

  email?: string | null;
  mobile?: string | null;
  phone?: string | null;

  address?: string | null;
  cep?: string | null;
  houseNumber?: string | null;
  complement?: string | null;
  city?: string | null;
  district?: string | null;
  uf?: string | null;

  rg?: string | null;
}
