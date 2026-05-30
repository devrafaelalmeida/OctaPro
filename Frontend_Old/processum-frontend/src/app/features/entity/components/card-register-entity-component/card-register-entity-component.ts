import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LucideAngularModule } from 'lucide-angular';


@Component({
  selector: 'app-card-register-entity-component',
  imports: [LucideAngularModule],
  templateUrl: './card-register-entity-component.html',
  styleUrl: './card-register-entity-component.css',
})
export class CardRegisterEntityComponent {

  @Input() title!: string;
  @Input() subtitle!: string;
  @Input() buttonText: string = 'Cadastrar';

  // svg ou icon
  @Input() icon: any;

  @Output() actionClick = new EventEmitter<void>();

  onClick() {
    this.actionClick.emit();
  }
}
