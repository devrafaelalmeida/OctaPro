import { Routes } from '@angular/router';
import { SettlementComponent } from '../view/settlements/list/settlement';
import { ReceiptsComponent } from '../view/receipts/receipts-component/receipts-component';
import { PaymentComponent } from '../view/payments/payment-component/payment-component';
import { SettlementFormComponent } from '../view/settlements/form/settlement-form-component/settlement-form-component';

export const FINANCE_ROUTES: Routes = [
  {
    path: 'financeiro',
    // component: FinanceComponent,
     children: [
      // {
      //   path: '',
      //   redirectTo: 'recebimentos',
      //   pathMatch: 'full'
      // },
      {
        path: 'recebimentos',
        component: ReceiptsComponent
      },
      {
        path: 'pagamentos',
        component: PaymentComponent
      },
      {
        path: 'acordos',
        // component: SettlementComponent,
        children: [
                  { path: '', component: SettlementComponent },
                  { path: 'novo', component: SettlementFormComponent },
                  { path: 'editar/:id_public_settlement', component: SettlementFormComponent },



                ]
      }
    ]
  }
];
