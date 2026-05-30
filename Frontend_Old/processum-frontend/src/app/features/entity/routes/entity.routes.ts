import { Routes } from '@angular/router';
import { EntityComponent } from '../view/entity/entity';
import { NewClientPfComponent } from '../view/new-client-pf-component/new-client-pf-component';
import { NewClientPjComponent } from '../view/new-client-pj-component/new-client-pj-component';

export const ENTITY_ROUTES: Routes = [
  {
    path: 'entidades',
    children: [
          { path: '', component: EntityComponent },
          { path: 'novo/pf', component: NewClientPfComponent },
          { path: 'editar/pf/:id_public_entity', component: NewClientPfComponent },

          { path: 'novo/pj', component: NewClientPjComponent },
          { path: 'editar/pj/:id_public_entity', component: NewClientPjComponent },


        ]
  }
];
