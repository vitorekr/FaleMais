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
  imports: [NgIf, FormsModule, HttpClientModule, CommonModule],
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
  errors: any = {};
  submitted = false;

  // 🔹 Estrutura de DDDs com comunicação válida
  validDestinations: { [key: string]: string[] } = {
    "011": ["016", "017", "018"],
    "016": ["011"],
    "017": ["011"],
    "018": ["011"]
  };

  availableDestinations: string[] = [];

  // 🔹 Getter para obter os DDDs de origem disponíveis
  get validOrigins(): string[] {
    return Object.keys(this.validDestinations);
  }

  constructor(private callCostService: CallCostService) { }

  calculateCost(): void {
    this.submitted = true;
    this.loading = true;
    this.errorMessage = '';
    this.result = null;

    this.validateFields();

    if (Object.keys(this.errors).length > 0) {
      this.loading = false;
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

  validateFields() {
    this.errors = {};

    if (!this.callCostRequest.origin) {
      this.errors.origin = 'O DDD de origem é obrigatório.';
    }

    if (!this.callCostRequest.destination) {
      this.errors.destination = 'O DDD de destino é obrigatório.';
    }

    if (!this.callCostRequest.duration) {
      this.errors.duration = 'O tempo da ligação é obrigatório.';
    } else if (isNaN(+this.callCostRequest.duration) || +this.callCostRequest.duration <= 0) {
      this.errors.duration = 'O tempo da ligação deve ser um número maior que zero.';
    }

    if (!this.callCostRequest.plan) {
      this.errors.plan = 'Selecione um plano antes de calcular.';
    }
  }

  // 🔹 Atualiza os DDDs de destino com base no DDD de origem selecionado
  updateDestinations() {
    const origin = this.callCostRequest.origin;
    this.availableDestinations = this.validDestinations[origin] || [];
    this.callCostRequest.destination = ''; // Resetar o destino ao alterar a origem
  }
}
