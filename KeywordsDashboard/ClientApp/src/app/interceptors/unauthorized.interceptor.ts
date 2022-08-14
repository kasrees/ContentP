import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { httpResponseStatusCodes } from '../models/httpResponseStatusCodes'

import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class UnauthorizedInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(tap(() => { },
    (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if ((err.status === httpResponseStatusCodes.Unauthorized)) {
          this.router.navigate(['/login']);
        } else {
          return;
        }
      }
    }));
  }
}
