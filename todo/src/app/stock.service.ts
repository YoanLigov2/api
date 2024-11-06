import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  private apiUrl = 'http://localhost:5001/api/stocks';

  constructor(private http: HttpClient) {}

  // Метод за вземане на последните 20 акции с цените в долари
  getLast20Stocks(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/last20`);
  }

  // Нов метод за вземане на цената на акцията в евро
  getStockPriceInEur(symbol: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${symbol}/priceInEur`);
  }
}
