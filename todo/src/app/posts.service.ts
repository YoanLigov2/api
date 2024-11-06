import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  private apiUrl = 'http://localhost:8000/api/example/';

  constructor(private http: HttpClient) { }

  // Method to fetch posts
  getPosts(): Observable<any> {
    return this.http.get(this.apiUrl + "getPosts");
  }

  loadPosts(): Observable<any> {
    return this.http.get(this.apiUrl);
  };

  clearPosts() {
    return this.http.delete(this.apiUrl + "deletePosts");
  }
}
