import { Injectable, inject } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';

import { Observable, throwError } from "rxjs";
import { catchError, finalize } from "rxjs/operators";
import { NgxSpinnerService } from 'ngx-spinner';

import { LocalStorageUtils } from '../utils/localstorage';
import { Router } from '@angular/router';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    private activeRequests = 0;

    private router = inject(Router);
    private spinner = inject(NgxSpinnerService);

    localStorageUtil = new LocalStorageUtils();

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        // increment and show spinner on first request
        this.activeRequests++;
        if (this.activeRequests === 1) {
            this.spinner.show();
        }

        return next.handle(req).pipe(
            catchError(error => {

                if (error instanceof HttpErrorResponse) {

                    if (error.status === 401) {
                        this.localStorageUtil.limparDadosLocaisUsuario();
                        this.router.navigate(['/conta/login'], { queryParams: { returnUrl: this.router.url }});
                    }
                    if (error.status === 403) {
                        this.router.navigate(['/acesso-negado']);
                    }
                }

                return throwError(error);
            }),
            finalize(() => {
                // decrement and hide spinner when no active requests
                this.activeRequests = Math.max(0, this.activeRequests - 1);
                if (this.activeRequests === 0) {
                    this.spinner.hide();
                }
            })
        );
    }

}