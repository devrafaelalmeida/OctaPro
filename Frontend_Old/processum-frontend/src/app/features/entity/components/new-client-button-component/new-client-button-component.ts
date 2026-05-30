import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { LucideAngularModule } from 'lucide-angular';
import { ClickOutsideDirective } from '../../../../shared/directives/ClickOutsideDirective';




@Component({
  selector: 'app-new-client-button-component',
  imports: [LucideAngularModule, ClickOutsideDirective],
  templateUrl: './new-client-button-component.html',
  styleUrl: './new-client-button-component.css',

})
export class NewClientButtonComponent {
  private router = inject(Router);

  dropdownOpen = false;

  toggleDropdown(){
    this.dropdownOpen = !this.dropdownOpen;
  }


  rotasCliente = new Map<string, string>([
    ['PF', '/entidades/novo/pf'],
    ['PJ', '/entidades/novo/pj']
  ]);


  newCliente(tipo: 'PF' | 'PJ'){

    this.dropdownOpen = false;

    const rota = this.rotasCliente.get(tipo);
    if (rota) {
      this.router.navigate([rota]);
    }

  }

}
