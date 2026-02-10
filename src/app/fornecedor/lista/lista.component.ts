import { Component, OnInit, inject } from '@angular/core';
import { FornecedorService } from '../services/fornecedor.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Fornecedor } from '../models/fornecedor';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html'
})
export class ListaComponent implements OnInit {

  public fornecedores: Fornecedor[];
  errorMessage: string;

  private fornecedorService = inject(FornecedorService);
  private spinner = inject(NgxSpinnerService);

  ngOnInit(): void {
    this.spinner.show();
    this.fornecedorService.obterTodos()
      .subscribe(
        fornecedores => { this.fornecedores = fornecedores; this.spinner.hide(); },
        error => { this.errorMessage = error; this.spinner.hide(); });
  }
}
