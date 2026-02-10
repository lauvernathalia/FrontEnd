import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProdutoService } from '../services/produto.service';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Produto } from '../models/produto';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-excluir',
  templateUrl: './excluir.component.html'
})
export class ExcluirComponent  {

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private produtoService = inject(ProdutoService);
  private toastr = inject(ToastrService);
  private spinner = inject(NgxSpinnerService);

  imagens: string = environment.imagensUrl;
  produto: Produto = this.route.snapshot.data['produto'];

  public excluirProduto() {
    this.spinner.show();
    this.produtoService.excluirProduto(this.produto.id)
      .subscribe(
      evento => { this.spinner.hide(); this.sucessoExclusao(evento) },
      ()     => { this.spinner.hide(); this.falha() }
      );
  }

  public sucessoExclusao(evento: any) {

    const toast = this.toastr.success('Produto excluido com Sucesso!', 'Good bye :D');
    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/produtos/listar-todos']);
      });
    }
  }

  public falha() {
    this.toastr.error('Houve um erro no processamento!', 'Ops! :(');
    this.spinner.hide();
  }
}

