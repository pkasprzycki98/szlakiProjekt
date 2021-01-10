import { AuthService } from './services/AuthService';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, filter, finalize, switchMap, take } from 'rxjs/operators';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  isRefreshingToken = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  constructor(
    private authService: AuthService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    req = this.addAuthorizationHeader(req);
    return next.handle(req).
      pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse) {
          const tokenExpired = error.headers.get('token-expired');
          if (tokenExpired) {
            return this.handleTokenExpired(req, next);
          }
           return throwError(error);
        }
      })
    );
  }

  private handleTokenExpired(req: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshingToken) {
      this.isRefreshingToken = true;

      this.tokenSubject.next(null);

      return this.authService.refreshToken(localStorage.getItem('JWT_TOKEN'),
        localStorage.getItem('REFRESH_TOKEN'))
        .pipe(
          switchMap((data) => {
            if (data) {
              localStorage.setItem('JWT_TOKEN', data.token);
              localStorage.setItem('REFRESH_TOKEN', data.refreshToken);
              this.tokenSubject.next(data.token);
              return next.handle(this.addAuthorizationHeader(req));
            }
            this.authService.logOut();
            return throwError('');
          }),
          catchError(error => {
            return of(<any>this.authService.logOut());
          }),
          finalize(() => {
            this.isRefreshingToken = false;
          })
        );
    } else {
      return this.tokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap((token) => {
          return next.handle(this.addAuthorizationHeader(req));
        }));
    }
  }

  private addAuthorizationHeader(req: HttpRequest<any>): HttpRequest<any> {
    const token = localStorage.getItem('JWT_TOKEN');
    if (token == null) {
      return req;
    }

    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    return clonedReq;
  }
}
