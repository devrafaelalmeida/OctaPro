import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { LucideAngularModule } from 'lucide-angular';
import { Menu, LogOut } from 'lucide-angular/src/icons';
import { LayoutService } from '../services/layout-service';
import { AuthService } from '../../core/service/auth-service';
const MODULES = [CommonModule, LucideAngularModule];


@Component({
  selector: 'app-navbar',
  imports: [...MODULES],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class NavbarComponent {
  protected readonly Menu = Menu;
  protected readonly LogOut = LogOut;
  private authService = inject(AuthService);

  constructor(public layoutService : LayoutService) {

  }

  sair(){
    this.authService.logout();
  }

}
