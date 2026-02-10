import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Usuario } from '../models/usuario';

import { Observable } from 'rxjs';
import { catchError, map, tap, finalize } from "rxjs/operators";
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseService } from 'src/app/services/base.service';

@Injectable()
export class ContaService extends BaseService {

    private http = inject(HttpClient);
    private spinner = inject(NgxSpinnerService);

    registrarUsuario(usuario: Usuario): Observable<Usuario> {
        let response = this.http
            .post(this.UrlServiceV1 + 'nova-conta', usuario, this.ObterHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(this.extractData),
                catchError(this.serviceError),
                finalize(() => this.spinner.hide())
            );

        return response;
    }

    login(usuario: Usuario): Observable<Usuario> {
        let response = this.http
            .post(this.UrlServiceV1 + 'entrar', usuario, this.ObterHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(this.extractData),
                catchError(this.serviceError),
                finalize(() => this.spinner.hide())
            );

        return response;
    }
}