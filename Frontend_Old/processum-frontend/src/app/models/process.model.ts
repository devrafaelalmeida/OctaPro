export interface Process {
  idPublic: string;
  processNumber: string;
  initialDate: string | null;
  respondent: string;
  description: string;
  isArchived: boolean;
  createdAt: string;
  natureAction: {
    id: number;
    text: string;
  };
  judicialAction: {
    id: number;
    text: string;
  };
  user: number;
  entities: any[];
}
