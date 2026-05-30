import { Routes } from '@angular/router';
import { MainComponent } from './layouts/main/main';
import { PROCESS_ROUTES } from './features/process/routes/process.routes';
import { ENTITY_ROUTES } from './features/entity/routes/entity.routes';
import { FINANCE_ROUTES } from './features/finance/routes/finance.routes';
import { FormLoginComponent } from './features/auth/components/form-login-component/form-login-component';
import { AUTH_ROUTES } from './features/auth/routes/auth.routes';
import { AuthGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  // LOGIN (SEM LAYOUT)
  ...AUTH_ROUTES,

  // ROTAS DO SISTEMA (COM LAYOUT)
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    children: [
      ...PROCESS_ROUTES,
      ...FINANCE_ROUTES,
      ...ENTITY_ROUTES
    ]
  }
];
