export interface ProcessEntity {
  idPublic: string;
  entityType: 'PF' | 'PJ'; // se existir apenas esses dois
  name: string;
}
