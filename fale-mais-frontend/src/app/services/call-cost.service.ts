import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CallCostRequest, CallCostResponse } from '../models/call-cost.model';

@Injectable({
  providedIn: 'root'
})
export class CallCostService {
  private apiUrl = 'http://localhost:5000/api/callcost/calculate';

  constructor(private http: HttpClient) { }

  calculateCost(request: CallCostRequest): Observable<CallCostResponse> {
    return this.http.post<CallCostResponse>(this.apiUrl, request);
  }
}
