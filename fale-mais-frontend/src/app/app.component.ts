import { Component } from '@angular/core';
import { CallCostCalculatorComponent } from './components/call-cost-calculator/call-cost-calculator.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CallCostCalculatorComponent], // Agora importamos os componentes diretamente
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'fale-mais-frontend';
}
