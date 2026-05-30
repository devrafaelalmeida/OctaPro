import { Component } from '@angular/core';
import { IAfterGuiAttachedParams } from 'ag-grid-community';
import { CommonModule } from '@angular/common';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-actions-menu-popup',
  standalone: true,
  imports: [CommonModule, LucideAngularModule],
  template: `
    <ul class="menu bg-base-100 rounded-box z-50 p-2 shadow min-w-[160px]">
      <li *ngFor="let action of actions">
        <a
          [ngClass]="action.class"
          (click)="handleAction(action)"
          (mousedown)="$event.stopPropagation()">
          <i-lucide [name]="action.icon" size="16"></i-lucide>
          {{ action.label }}
        </a>
      </li>
    </ul>
  `,
  styles: [`
    :host {
      display: block;
    }

    ul {
      user-select: none;
    }

    li a {
      cursor: pointer;
    }
  `]
})
export class ActionsMenuPopupComponent {
  params: any;
  actions: any[] = [];
  private hidePopupFunc?: () => void;

  agInit(params: any): void {
    this.params = params;
    this.actions = params.actions || [];
    this.hidePopupFunc = params.hidePopup;
  }

  afterGuiAttached(params?: IAfterGuiAttachedParams): void {
    // Focar no primeiro item ao abrir
    if (params?.hidePopup) {
      this.hidePopupFunc = params.hidePopup;
    }
  }

  handleAction(action: any): void {
    // Executar a ação
    if (action.onClick) {
      action.onClick(this.params.rowData);
    }

    // Fechar o popup
    if (this.hidePopupFunc) {
      this.hidePopupFunc();
    } else if (this.params.hidePopup) {
      this.params.hidePopup();
    }
  }
}
