import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { LucideAngularModule } from "lucide-angular";

@Component({
  selector: 'app-new-process-button',
  imports: [LucideAngularModule],
  templateUrl: './new-process-button.html',
  styleUrl: './new-process-button.css',
})
export class NewProcessButton {

  private router = inject(Router);

  onNewProcess() {
    this.router.navigate(['/processos/novo']);
  }

}
