import { ProcessEntity } from "../models/process-entity.model";

export interface ProcessRequest {

  processNumber: string;

  initialDate: string;

  respondent: string;

  description?: string;

  natureActionId: number;

  judicialActionId: number;

  EntityIds: ProcessEntity[];
}
