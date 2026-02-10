import { Component, inject } from '@angular/core';
import { Fornecedor } from '../models/fornecedor';

import { ActivatedRoute, Router } from '@angular/router';
import { FornecedorService } from '../services/fornecedor.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-excluir',
  templateUrl: './excluir.component.html'
})
export class ExcluirComponent {

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fornecedorService = inject(FornecedorService);
  private toastr = inject(ToastrService);
  private sanitizer = inject(DomSanitizer);
  private spinner = inject(NgxSpinnerService);

  fornecedor: Fornecedor = this.route.snapshot.data['fornecedor'];
  enderecoMap = this.sanitizer.bypassSecurityTrustResourceUrl("https://www.google.com/maps/embed/v1/place?q=" + this.EnderecoCompleto() + "&key=AIzaSyAP0WKpL7uTRHGKWyakgQXbW6FUhrrA5pE");
  errors: any[] = [];

  public EnderecoCompleto(): string {
    return this.fornecedor.endereco.logradouro + ", " + this.fornecedor.endereco.numero + " - " + this.fornecedor.endereco.bairro + ", " + this.fornecedor.endereco.cidade + " - " + this.fornecedor.endereco.estado;
  }

  excluirEvento() {
    this.spinner.show();
    this.fornecedorService.excluirFornecedor(this.fornecedor.id)
      .subscribe(
        fornecedor => { this.spinner.hide(); this.sucessoExclusao(fornecedor) },
        error => { this.spinner.hide(); this.falha(error) }
      );
  }

  sucessoExclusao(evento: any) {

    const toast = this.toastr.success('Fornecedor excluido com Sucesso!', 'Good bye :D');
    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/fornecedores/listar-todos']);
      });
    }
  }

  falha(fail) {
    this.errors = fail.error.errors;
    this.toastr.error('Houve um erro no processamento!', 'Ops! :(');
    this.spinner.hide();
  }
}
