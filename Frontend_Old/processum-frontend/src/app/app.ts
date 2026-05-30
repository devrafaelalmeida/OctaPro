import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PrimeNG } from 'primeng/config';
import { PRIME_NG_LOCALE_PT_BR } from './shared/prime-ng/prime-ng-locale-pt';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  private primeng = inject(PrimeNG);

  ngOnInit() {
    this.primeng.setTranslation(PRIME_NG_LOCALE_PT_BR);
  }
}
