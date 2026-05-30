import { CommonModule } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { Router } from '@angular/router';
import { LucideAngularModule } from "lucide-angular";

@Component({
  selector: 'app-button-new-register-component',
  imports: [LucideAngularModule, CommonModule],
  templateUrl: './button-new-register-component.html',
  styleUrl: './button-new-register-component.css',
})
export class ButtonNewRegisterComponent {

  private router = inject(Router);

  @Input() route?: string;
  @Input() title: string = '';
  @Input() icon: string = 'plus';
  @Input() customClass: string = 'bg-primary-600 text-white';
  @Input() action?: () => void;

  handleClick() {
    if (this.action) {
      this.action();
      return;
    }

    if (this.route) {
      this.router.navigate([this.route]);
    }
  }

}
