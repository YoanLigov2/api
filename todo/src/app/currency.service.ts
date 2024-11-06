import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {
  private apiUrl = 'http://localhost:8000/api/values/convert';

  constructor(private http: HttpClient) { }

  convertCurrency(from: string, to: string, amount: number): Observable<number> {
    let params = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('amount', amount.toString());

    return this.http.get<number>(this.apiUrl, { params });
  }
}
