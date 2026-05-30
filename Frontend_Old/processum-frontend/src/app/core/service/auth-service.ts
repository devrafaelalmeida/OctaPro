import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../shared/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);
  private router = inject(Router);

  private API = environment.apiUrl;

  login(email: string, password: string): Observable<any> {

    return this.http.post(`${this.API}/auth/login`, {
      email,
      password
    }).pipe(
      tap((response: any) => {
        localStorage.setItem('access_token', response.token);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('access_token');
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  isLogged(): boolean {
    return !!this.getToken();
  }
}
