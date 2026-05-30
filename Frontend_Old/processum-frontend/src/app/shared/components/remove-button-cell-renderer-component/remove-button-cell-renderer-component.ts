import { Component } from '@angular/core';
import { LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-remove-button-cell-renderer-component',
  imports: [LucideAngularModule],
  templateUrl: './remove-button-cell-renderer-component.html',
  styleUrl: './remove-button-cell-renderer-component.css',
})
export class RemoveButtonCellRendererComponent {
private params!: any;

  agInit(params: any): void {
    this.params = params;
  }

  refresh(): boolean {
    return false;
  }

  onRemove() {
    this.params.onRemove(this.params.data);
  }
}
