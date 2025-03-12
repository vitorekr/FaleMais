export interface CallCostRequest {
  origin: string;
  destination: string;
  duration: number;
  plan: string;
}

export interface CallCostResponse {
  costWithPlan: number;
  costWithoutPlan: number;
}
