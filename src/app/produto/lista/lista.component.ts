import { Component, OnInit, inject } from '@angular/core';
import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produto.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html'
})
export class ListaComponent implements OnInit {

  imagens: string = environment.imagensUrl;

  public produtos: Produto[];
  errorMessage: string;

  private produtoService = inject(ProdutoService);
  private spinner = inject(NgxSpinnerService);

  ngOnInit(): void {
    this.spinner.show();
    this.produtoService.obterTodos()
      .subscribe(
        produtos => { this.produtos = produtos; this.spinner.hide(); },
        error => { this.errorMessage = error; this.spinner.hide(); });
  }
}
