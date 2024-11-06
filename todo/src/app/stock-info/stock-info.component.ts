import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-stock-info',
  templateUrl: './stock-info.component.html',
  styleUrls: ['./stock-info.component.css']
})
export class StockInfoComponent {
  symbol: string = '';
  stockInfo: any;
  displayedColumns: string[] = ['ticker', 'name', 'exchange', 'industry', 'marketCapitalization'];

  constructor(private http: HttpClient) {}

  fetchStockInfo() {
    if (this.symbol) {
      this.http.get(`https://localhost:5001/api/stock/${this.symbol}`)
        .subscribe(
          (data) => {
            this.stockInfo = data;
          },
          (error) => {
            console.error('Грешка при извличане на информация за акцията', error);
          }
        );
    }
  }
}
