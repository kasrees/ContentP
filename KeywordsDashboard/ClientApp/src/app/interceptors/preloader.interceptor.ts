import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { PreloaderService } from '../services/preloader.service';
import { finalize } from 'rxjs/operators';

@Injectable()
export class PreloaderInterceptor implements HttpInterceptor {
  
  private minSpinningTime: number = 700; 

  constructor(private preloaderService: PreloaderService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
     let startRequestTime = Date.now();
     this.preloaderService.show();
     return next.handle(request).pipe(
        finalize(() => {
          let endRequestTime = Date.now();
          let timeout = (endRequestTime - startRequestTime) > this.minSpinningTime ? 0 : this.minSpinningTime - (endRequestTime - startRequestTime);
          setTimeout( () => this.preloaderService.hide(), timeout);
        }),
     );
  }
}
