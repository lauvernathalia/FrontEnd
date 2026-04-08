import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';
import { NgxSpinnerService } from 'ngx-spinner';

import { BaseService } from 'src/app/services/base.service';

@Injectable()
export class DashrepService extends BaseService {

  private http = inject(HttpClient);
  private spinner = inject(NgxSpinnerService);

  // Exemplo de método para buscar dados do dashboard
  obterDadosDashboard(): Observable<any> {
    return this.http
      .get<any>(this.UrlServiceV1 + 'dashboard', this.ObterAuthHeaderJson())
      .pipe(
        tap(() => this.spinner.show()),
        catchError(super.serviceError),
        finalize(() => this.spinner.hide())
      );
  }
}
