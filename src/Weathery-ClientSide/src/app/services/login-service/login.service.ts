import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  login(data): Observable<any>{
    return this.http.post('https://localhost:5001/api/authentication/login', data);
  }

  saveToken(token){
    localStorage.setItem('token', token);
  }

  getToken(){
    localStorage.getItem('token');
  }
}
