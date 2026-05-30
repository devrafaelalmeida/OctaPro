import { EntityCompany } from "./entity-company.model";
import { EntityIndividual } from "./entity-individual.model";

export interface  Entity {

  entityType: string;

  // statusId: number;

  idPublic: string;

  createdAt: string;

  updatedAt: string;

  entityIndividual?: EntityIndividual;

  entityCompany?: EntityCompany;

  // legalFeesInstallments: LegalFeesInstallment[];

  // judicialProcessEntities: JudicialProcessEntity[];

  // legalFeeEntities: LegalFeeEntity[];
}
