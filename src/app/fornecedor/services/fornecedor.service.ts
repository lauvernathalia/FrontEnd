import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Observable } from "rxjs";
import { catchError, map, tap, finalize } from "rxjs/operators";
import { NgxSpinnerService } from 'ngx-spinner';

import { BaseService } from 'src/app/services/base.service';
import { Fornecedor } from '../models/fornecedor';
import { CepConsulta, Endereco } from '../models/endereco';

@Injectable()
export class FornecedorService extends BaseService {

    fornecedor: Fornecedor = new Fornecedor();

    private http = inject(HttpClient);
    private spinner = inject(NgxSpinnerService);

    obterTodos(): Observable<Fornecedor[]> {
        return this.http
            .get<Fornecedor[]>(this.UrlServiceV1 + "fornecedores")
            .pipe(
                tap(() => this.spinner.show()),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    obterPorId(id: string): Observable<Fornecedor> {
        return this.http
            .get<Fornecedor>(this.UrlServiceV1 + "fornecedores/" + id, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    novoFornecedor(fornecedor: Fornecedor): Observable<Fornecedor> {
        return this.http
            .post(this.UrlServiceV1 + "fornecedores", fornecedor, this.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(super.extractData),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    atualizarFornecedor(fornecedor: Fornecedor): Observable<Fornecedor> {
        return this.http
            .put(this.UrlServiceV1 + "fornecedores/" + fornecedor.id, fornecedor, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(super.extractData),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    excluirFornecedor(id: string): Observable<Fornecedor> {
        return this.http
            .delete(this.UrlServiceV1 + "fornecedores/" + id, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(super.extractData),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    atualizarEndereco(endereco: Endereco): Observable<Endereco> {
        return this.http
            .put(this.UrlServiceV1 + "fornecedores/endereco/" + endereco.id, endereco, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(super.extractData),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    consultarCep(cep: string): Observable<CepConsulta> {
        return this.http
            .get<CepConsulta>(`https://viacep.com.br/ws/${cep}/json/`)
            .pipe(
                tap(() => this.spinner.show()),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            )
    }
}
