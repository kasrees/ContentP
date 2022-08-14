import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }
  
  login(login: string, password: string) {
    return this.http.post(environment.apiUrl + 'api/account/auth', {Login: login, Password: password}).pipe(
      catchError((error) => {
        return throwError(error);
      })
    )
  }
}
