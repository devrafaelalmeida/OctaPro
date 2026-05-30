import { Routes } from '@angular/router';
import { FormLoginComponent } from '../components/form-login-component/form-login-component';
import { FormResetPasswordComponent } from '../components/form-reset-password-component/form-reset-password-component';
import { AuthView } from '../view/login/auth-view';


export const AUTH_ROUTES: Routes = [
  {
    path: 'login',
    component: AuthView,
    children: [
      { path: '', component: FormLoginComponent },
      { path: 'reset-password', component: FormResetPasswordComponent },


    ]
  }
];
