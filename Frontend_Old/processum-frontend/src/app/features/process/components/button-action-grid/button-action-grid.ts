import { CommonModule } from '@angular/common';
import { Component, ComponentRef, Injector, createComponent, ApplicationRef, EnvironmentInjector } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { LucideAngularModule } from "lucide-angular";
import { Ellipsis } from 'lucide-angular/src/icons';
import { ActionsMenuPopupComponent } from '../menu-popup/menu-popup';

const MODULES = [CommonModule];

@Component({
  selector: 'app-button-action-grid',
  imports: [...MODULES, LucideAngularModule],
  templateUrl: './button-action-grid.html',
  styleUrl: './button-action-grid.css'
})
export class ButtonActionGrid implements ICellRendererAngularComp {
  params!: any;
  actions!: any[];
  useDropdown = false;
  protected readonly Ellipsis = Ellipsis;

  private popupComponentRef?: ComponentRef<ActionsMenuPopupComponent>;
  private hidePopupFunc?: () => void;
  private clickOutsideListener?: (e: MouseEvent) => void;
  private popupElement?: HTMLElement;

  constructor(
    private injector: Injector,
    private appRef: ApplicationRef,
    private environmentInjector: EnvironmentInjector
  ) {}

  agInit(params: any): void {
    this.params = params;
    this.actions = params.actions;
    this.useDropdown = params.useDropdown ?? false;
  }

  refresh(): boolean {
    return false;
  }

  showPopup(event: MouseEvent): void {
    event.stopPropagation();
    event.preventDefault();

    // Se já existe um popup aberto, fecha primeiro
    if (this.popupComponentRef) {
      this.hidePopup();
      return;
    }

    const button = event.currentTarget as HTMLElement;

    // Criar o componente do popup
    this.popupComponentRef = createComponent(ActionsMenuPopupComponent, {
      environmentInjector: this.environmentInjector
    });

    // Configurar os parâmetros do popup
    this.popupComponentRef.instance.agInit({
      actions: this.actions,
      rowData: this.params.data,
      hidePopup: () => this.hidePopup()
    });

    // Anexar o componente ao DOM
    this.appRef.attachView(this.popupComponentRef.hostView);
    this.popupElement = this.popupComponentRef.location.nativeElement;

    // Verificar se o elemento foi criado
    if (!this.popupElement) {
      console.error('Falha ao criar o elemento do popup');
      this.hidePopup();
      return;
    }

    // Tentar usar o popupService do AG-Grid
    const popupService = this.getPopupService();

    if (popupService) {
      this.useAgGridPopupService(popupService, button);
    } else {
      this.positionPopupManually(this.popupElement, button);
    }

    // Adicionar listener para fechar ao clicar fora (backup)
    this.addClickOutsideListener(button);
  }

  private getPopupService(): any {
    try {
      return (this.params.api as any).gridOptionsService?.get('popupService');
    } catch {
      return null;
    }
  }

  private useAgGridPopupService(popupService: any, button: HTMLElement): void {
    const popupParams = {
      eChild: this.popupElement,
      eParent: document.body,
      type: 'columnMenu',
      eventSource: button,
      position: 'under',
      alignSide: 'right',
      keepWithinBounds: true,
      column: this.params.column,
      rowNode: this.params.node
    };

    this.hidePopupFunc = popupService.addPopup(popupParams);
  }

  private positionPopupManually(popupElement: HTMLElement, button: HTMLElement): void {
    const rect = button.getBoundingClientRect();
    const popupWidth = 160;

    popupElement.style.position = 'fixed';
    popupElement.style.top = `${rect.bottom + 4}px`;
    popupElement.style.left = `${rect.left - popupWidth + rect.width}px`;
    popupElement.style.zIndex = '9999';

    document.body.appendChild(popupElement);
  }

  private addClickOutsideListener(button: HTMLElement): void {
    // Aguardar um pouco antes de adicionar o listener
    setTimeout(() => {
      this.clickOutsideListener = (e: MouseEvent) => {
        const target = e.target as Node;

        if (this.popupElement &&
            !this.popupElement.contains(target) &&
            !button.contains(target)) {
          this.hidePopup();
        }
      };

      document.addEventListener('click', this.clickOutsideListener);

      // Também fechar ao rolar a grid
      try {
        const gridElement = button.closest('.ag-root-wrapper');
        const viewport = gridElement?.querySelector('.ag-body-viewport');

        if (viewport) {
          viewport.addEventListener('scroll', () => this.hidePopup(), { once: true });
        }
      } catch (e) {
        // Fallback: tentar através da API
        const viewport = this.params.api.gridPanel?.eBodyViewport;
        if (viewport) {
          viewport.addEventListener('scroll', () => this.hidePopup(), { once: true });
        }
      }
    }, 150);
  }

  private hidePopup(): void {
    // Remover listener de click
    if (this.clickOutsideListener) {
      document.removeEventListener('click', this.clickOutsideListener);
      this.clickOutsideListener = undefined;
    }

    // Chamar a função de hide do AG-Grid se existir
    if (this.hidePopupFunc) {
      try {
        this.hidePopupFunc();
      } catch (e) {
        console.warn('Error hiding popup:', e);
      }
      this.hidePopupFunc = undefined;
    }

    // Remover do DOM se foi adicionado manualmente (fazer isso ANTES de destruir)
    if (this.popupElement?.parentNode === document.body) {
      try {
        document.body.removeChild(this.popupElement);
      } catch (e) {
        console.warn('Error removing popup element:', e);
      }
    }

    // Destruir o componente
    if (this.popupComponentRef) {
      try {
        this.appRef.detachView(this.popupComponentRef.hostView);
        this.popupComponentRef.destroy();
      } catch (e) {
        console.warn('Error destroying popup component:', e);
      }
      this.popupComponentRef = undefined;
    }

    this.popupElement = undefined;
  }

  ngOnDestroy(): void {
    this.hidePopup();
  }
}
