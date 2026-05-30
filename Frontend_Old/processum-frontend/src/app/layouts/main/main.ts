import { Component, HostListener } from '@angular/core';
import { SidebarComponent } from '../sidebar/sidebar';
import { LayoutService } from '../services/layout-service';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar';
import { RouterOutlet } from '@angular/router';

const COMPONENTS = [SidebarComponent, NavbarComponent ];
const MODULES = [CommonModule, RouterOutlet];

@Component({
  selector: 'app-main',
  imports: [...COMPONENTS, ...MODULES],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class MainComponent {
  constructor(public layoutService: LayoutService) {}

  isDesktop = window.innerWidth >= 1024;

  @HostListener('window:resize')
  onResize() {
    this.isDesktop = window.innerWidth >= 1024;
  }
}
