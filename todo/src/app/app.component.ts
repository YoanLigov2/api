import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient, HttpParams } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PostsService } from './posts.service';
import { CurrencyService } from './currency.service';
import { StockService } from './stock.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { forkJoin, map } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [PostsService, CurrencyService, StockService]
})
export class AppComponent { 
  stocks: any[] = [];
  // displayedColumns: string[] = ['symbol', 'date', 'priceInUsd', 'priceInEur', 'retrievedAt'];
  tasks:any=[];
  posts: any[] = [];
  newtask="";
  fromCurrency = 'EUR';
  toCurrency = 'BGN';
  amount = 50;
  convertedAmount: number | undefined;

  APIURL="http://localhost:8000/";

  symbol: string = '';
  stockInfo: any;
  displayedColumns: string[] = ['ticker', 'name', 'exchange', 'industry', 'marketCapitalization'];

  constructor(private http:HttpClient, private postsService: PostsService, private currencyService: CurrencyService, private stockService: StockService) {}

  ngOnInit() {
    this.stockService.getLast20Stocks().subscribe(stocks => {
      this.stocks = stocks;

      // Добавяме цените в евро за всяка акция
      this.stocks.forEach(stock => {
        this.stockService.getStockPriceInEur(stock.symbol).subscribe(priceInEur => {
          stock.priceInEur = priceInEur.priceInEur; // Запазване на цената в евро в масивa
        });
      });
    });

    // this.get_tasks();
    // this.postsService.getPosts().subscribe((data: any) => {
    //   this.posts = data;
    // });
  };

  fetchStockInfo() {
    if (this.symbol) {
      this.http.get(`http://localhost:5001/api/finnhub/${this.symbol}`)
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

  get_posts() {
    this.postsService.loadPosts().subscribe(
      (data) => {
        this.posts = data;  
        window.location.reload();
      }
    );
  }

  delete_posts() {
    this.postsService.clearPosts().subscribe(
      (res) => {
        window.location.reload();
      }
    );
  }

  get_tasks() {
    this.http.get(this.APIURL + "get_tasks").subscribe((res) => {
      this.tasks = res;
    })
  };

  add_task() {
    let body = new FormData();
    body.append('task', this.newtask);
    this.http.post(this.APIURL + "add_task", body).subscribe((res) => {
      alert(res);
      this.newtask= "";
      this.get_tasks();
    })
  };

  delete_task(id: any) {
    this.http.delete(`${this.APIURL}delete_task/${id}`).subscribe(
      (res) => {
        alert(res);
        this.get_tasks();
      },
      (error) => {
        console.error('Error occurred:', error);
        alert('An error occurred while deleting the task.');
      }
    );
  }

  convert(): void {
    this.currencyService.convertCurrency(this.fromCurrency, this.toCurrency, this.amount).subscribe(
      (res) => {
        this.convertedAmount = res;
        console.log(res);
        
      },
      (error) => console.error('Error:', error)
    );
  }
}
