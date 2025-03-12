import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallCostCalculatorComponent } from './call-cost-calculator.component';

describe('CallCostCalculatorComponent', () => {
  let component: CallCostCalculatorComponent;
  let fixture: ComponentFixture<CallCostCalculatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CallCostCalculatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CallCostCalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
