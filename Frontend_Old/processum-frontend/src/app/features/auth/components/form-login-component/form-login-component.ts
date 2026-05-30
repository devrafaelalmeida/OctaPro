import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/service/auth-service';
import { Router } from '@angular/router';

const MODULES = [ ReactiveFormsModule ];

@Component({
  selector: 'app-form-login-component',
  imports: [...MODULES],
  templateUrl: './form-login-component.html',
  styleUrl: './form-login-component.css',
  standalone: true
})
export class FormLoginComponent {

  private formGroup = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  form = this.formGroup.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  onSubmit() {

    if (this.form.invalid) {
      return;
    }

    const { email, password } = this.form.value;
    console.log("Email:", email);
    console.log("Password:", password);

    this.authService.login(email!, password!).subscribe({
      next: () => {
        console.log("Login realizado");
        this.router.navigate(['/processos']);
      },
      error: (err) => {
        console.error("Erro no login", err);
      }
    });

  }

}
