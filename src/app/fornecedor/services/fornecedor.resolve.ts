import { Injectable, inject } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Fornecedor } from '../models/fornecedor';
import { FornecedorService } from './fornecedor.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators';

@Injectable()
export class FornecedorResolve  {

    private fornecedorService = inject(FornecedorService);
    private spinner = inject(NgxSpinnerService);

    resolve(route: ActivatedRouteSnapshot) {
        this.spinner.show();
        return this.fornecedorService.obterPorId(route.params['id'])
            .pipe(finalize(() => this.spinner.hide()));
    }
}