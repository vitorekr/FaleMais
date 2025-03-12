import { Component } from '@angular/core';
import { CallCostService } from '../../services/call-cost.service';
import { CallCostRequest, CallCostResponse } from '../../models/call-cost.model';
import { HttpClientModule } from '@angular/common/http';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-call-cost-calculator',
  standalone: true,
  imports: [NgIf, FormsModule, HttpClientModule], // Agora importa os módulos necessários
  templateUrl: './call-cost-calculator.component.html',
  styleUrls: ['./call-cost-calculator.component.css'],
  providers: [CallCostService]
})
export class CallCostCalculatorComponent {
  callCostRequest: CallCostRequest = {
    origin: '',
    destination: '',
    duration: 0,
    plan: ''
  };
  result: CallCostResponse | null = null;
  errorMessage: string = '';
  loading = false;

  constructor(private callCostService: CallCostService) { }

  calculateCost(): void {
    this.loading = true;
    this.errorMessage = '';
    this.result = null;

    if (!this.callCostRequest.origin || !this.callCostRequest.destination || this.callCostRequest.duration <= 0 || !this.callCostRequest.plan) {
      this.errorMessage = 'Todos os campos são obrigatórios!';
      return;
    }

    this.callCostService.calculateCost(this.callCostRequest).subscribe({
      next: (response) => {
        this.result = response;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Erro ao calcular o custo da chamada. Verifique os dados informados.';
        this.loading = false;
      }
    });
  }
}
