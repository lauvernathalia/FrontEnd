import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Observable } from "rxjs";
import { catchError, map, tap, finalize } from "rxjs/operators";
import { NgxSpinnerService } from 'ngx-spinner';

import { BaseService } from 'src/app/services/base.service';
import { Produto, Fornecedor } from '../models/produto';

@Injectable()
export class ProdutoService extends BaseService {

    private http = inject(HttpClient);
    private spinner = inject(NgxSpinnerService);

    obterTodos(): Observable<Produto[]> {
        return this.http
            .get<Produto[]>(this.UrlServiceV1 + "produtos", super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    obterPorId(id: string): Observable<Produto> {
        return this.http
            .get<Produto>(this.UrlServiceV1 + "produtos/" + id, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    novoProduto(produto: Produto): Observable<Produto> {
        return this.http
            .post(this.UrlServiceV1 + "produtos", produto, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(super.extractData),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    atualizarProduto(produto: Produto): Observable<Produto> {
        return this.http
            .put(this.UrlServiceV1 + "produtos/" + produto.id, produto, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(super.extractData),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }

    excluirProduto(id: string): Observable<Produto> {
        return this.http
            .delete(this.UrlServiceV1 + "produtos/" + id, super.ObterAuthHeaderJson())
            .pipe(
                tap(() => this.spinner.show()),
                map(super.extractData),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }    

    obterFornecedores(): Observable<Fornecedor[]> {
        return this.http
            .get<Fornecedor[]>(this.UrlServiceV1 + "fornecedores")
            .pipe(
                tap(() => this.spinner.show()),
                catchError(super.serviceError),
                finalize(() => this.spinner.hide())
            );
    }
}
