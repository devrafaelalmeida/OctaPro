import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { LucideAngularModule } from 'lucide-angular';
import { Scale, BadgeDollarSign, ChevronLeft, ChevronRight, BookUser, Menu } from 'lucide-angular/src/icons';
import { LayoutService } from '../services/layout-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';



const MODULES = [CommonModule, LucideAngularModule, RouterLink, RouterLinkActive];

@Component({
  selector: 'app-sidebar',
  imports: [...MODULES],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class SidebarComponent {
  protected readonly Scale = Scale;
  protected readonly BadgeDollarSign = BadgeDollarSign;
  protected readonly ChevronLeft = ChevronLeft;
  protected readonly ChevronRight = ChevronRight;
  protected readonly BookUser = BookUser;
  protected readonly Menu = Menu;

  constructor(public layoutService : LayoutService,
    private router: Router
  ) {}


  menu = [
    {
      label: 'Processos',
      iconName: 'scale',
      route: '/processos'
    },
    {
      label: 'Financeiro',
      iconName: 'badge-dollar-sign',
      open: false,
      children: [
        { label: 'Recebimentos', route: '/financeiro/recebimentos' },
        { label: 'Pagamentos', route: '/financeiro/pagamentos' },
        { label: 'Acordos', route: '/financeiro/acordos' }
      ]
    },
    {
      label: 'Entidades',
      iconName: 'book-user',
      route: '/entidades'
    }
  ];
  toggleItem(item: any) {
  this.menu.forEach(i => {
    if (i !== item) i.open = false;
  });
  item.open = !item.open;
}

ngOnInit() {
  const currentUrl = this.router.url;

  this.menu.forEach(item => {
    if (item.children) {
      item.open = item.children.some(c =>
        currentUrl.includes(c.route)
      );
    }
  });
}


}
