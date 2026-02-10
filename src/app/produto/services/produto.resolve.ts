import { Injectable, inject } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Produto } from '../models/produto';
import { ProdutoService } from './produto.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators';

@Injectable()
export class ProdutoResolve  {

    private produtoService = inject(ProdutoService);
    private spinner = inject(NgxSpinnerService);

    resolve(route: ActivatedRouteSnapshot) {
        this.spinner.show();
        return this.produtoService.obterPorId(route.params['id'])
            .pipe(finalize(() => this.spinner.hide()));
    }
}