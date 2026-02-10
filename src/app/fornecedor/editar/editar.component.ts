import { Component, OnInit, ViewChildren, ElementRef, inject } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormControlName, AbstractControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';

import { StringUtils } from 'src/app/utils/string-utils';
import { Fornecedor } from '../models/fornecedor';
import { Endereco, CepConsulta } from '../models/endereco';
import { FornecedorService } from '../services/fornecedor.service';
import { FormBaseComponent } from 'src/app/base-components/form-base.component';

@Component({
  selector: 'app-editar',
  templateUrl: './editar.component.html'
})
export class EditarComponent extends FormBaseComponent implements OnInit {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  errors: any[] = [];
  errorsEndereco: any[] = [];
  fornecedorForm: UntypedFormGroup;
  enderecoForm: UntypedFormGroup;

  fornecedor: Fornecedor = new Fornecedor();
  endereco: Endereco = new Endereco();

  textoDocumento: string = '';
  // Máscara combinada CPF/CNPJ: o ngx-mask escolhe o padrão conforme o tamanho
  documentoMask: string = '000.000.000-00|00.000.000/0000-00';
  tipoFornecedor: number;

  private fb = inject(UntypedFormBuilder);
  private fornecedorService = inject(FornecedorService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  private route = inject(ActivatedRoute);
  private config = inject(NgbModalConfig);
  private modalService = inject(NgbModal);
  private spinner = inject(NgxSpinnerService);

  constructor() {
    super();
    this.config.backdrop = 'static';
    this.config.keyboard = false;

    this.validationMessages = {
      nome: {
        required: 'Informe o Nome',
      },
      documento: {
        required: 'Informe o Documento',
        cpf: 'CPF em formato inválido',
        cnpj: 'CNPJ em formato inválido'
      },
      logradouro: {
        required: 'Informe o Logradouro',
      },
      numero: {
        required: 'Informe o Número',
      },
      bairro: {
        required: 'Informe o Bairro',
      },
      cep: {
        required: 'Informe o CEP',
        cep: 'CEP em formato inválido',
      },
      cidade: {
        required: 'Informe a Cidade',
      },
      estado: {
        required: 'Informe o Estado',
      }
    };

    super.configurarMensagensValidacaoBase(this.validationMessages);

    this.fornecedor = this.route.snapshot.data['fornecedor'];
    this.tipoFornecedor = this.fornecedor.tipoFornecedor;
  }

  ngOnInit() {

    // spinner removed during migration; consider re-adding a compatible loader

    this.fornecedorForm = this.fb.group({
      id: '',
      nome: ['', [Validators.required]],
      documento: '',
      ativo: ['', [Validators.required]],
      tipoFornecedor: ['', [Validators.required]]
    });

    this.enderecoForm = this.fb.group({
      id: '',
      logradouro: ['', [Validators.required]],
      numero: ['', [Validators.required]],
      complemento: [''],
      bairro: ['', [Validators.required]],
      cep: ['', [Validators.required]],
      cidade: ['', [Validators.required]],
      estado: ['', [Validators.required]],
      fornecedorId: ''
    });

    this.preencherForm();

    // spinner removed during migration; consider re-adding a compatible loader
  }

  preencherForm() {

    this.fornecedorForm.patchValue({
      id: this.fornecedor.id,
      nome: this.fornecedor.nome,
      ativo: this.fornecedor.ativo,
      tipoFornecedor: this.fornecedor.tipoFornecedor.toString(),
      documento: this.fornecedor.documento
    });

    if (this.tipoFornecedorForm().value === "1") {
      this.documento().setValidators([Validators.required]);
      this.textoDocumento = 'CPF (requerido)';
    }
    else {
      this.documento().setValidators([Validators.required]);
      this.textoDocumento = 'CNPJ (requerido)';
    }

    this.documento().updateValueAndValidity();

    this.enderecoForm.patchValue({
      id: this.fornecedor.endereco.id,
      logradouro: this.fornecedor.endereco.logradouro,
      numero: this.fornecedor.endereco.numero,
      complemento: this.fornecedor.endereco.complemento,
      bairro: this.fornecedor.endereco.bairro,
      cep: this.fornecedor.endereco.cep,
      cidade: this.fornecedor.endereco.cidade,
      estado: this.fornecedor.endereco.estado
    });
  }

  ngAfterViewInit() {
    this.tipoFornecedorForm().valueChanges.subscribe(() => {
      this.trocarValidacaoDocumento();
      super.configurarValidacaoFormularioBase(this.formInputElements, this.fornecedorForm)
      super.validarFormulario(this.fornecedorForm)
    });

    this.documento().valueChanges
      .subscribe((value: string) => {
        this.ajustarTipoFornecedorPorDocumento(value);
      });

    super.configurarValidacaoFormularioBase(this.formInputElements, this.fornecedorForm);
  }

  trocarValidacaoDocumento() {

    if (this.tipoFornecedorForm().value === "1") {
      this.documento().clearValidators();
      this.documento().setValidators([Validators.required]);
      this.textoDocumento = 'CPF (requerido)';
    }

    else {
      this.documento().clearValidators();
      this.documento().setValidators([Validators.required]);
      this.textoDocumento = 'CNPJ (requerido)';
    }

    this.documento().updateValueAndValidity();
  }

  onDocumentoBlur(valor: string) {
    this.ajustarTipoFornecedorPorDocumento(valor);
  }

  private ajustarTipoFornecedorPorDocumento(valor: string) {
    if (!valor) {
      return;
    }

    const somenteDigitos = StringUtils.somenteNumeros(valor);
    if (!somenteDigitos) {
      return;
    }

    let tipoDetectado: string | null = null;

    // Regra mais precisa:
    //  - exatamente 11 dígitos -> CPF
    //  - 14 ou mais dígitos -> CNPJ
    if (somenteDigitos.length === 11) {
      tipoDetectado = '1'; // Pessoa Física (CPF)
    } else if (somenteDigitos.length >= 14) {
      tipoDetectado = '2'; // Pessoa Jurídica (CNPJ)
    } else {
      // Documento ainda incompleto, não força troca de tipo
      return;
    }

    if (!tipoDetectado) {
      return;
    }

    const tipoAtual = this.tipoFornecedorForm().value;
    if (tipoAtual !== tipoDetectado) {
      this.tipoFornecedorForm().setValue(tipoDetectado);
    }
  }

  documento(): AbstractControl {
    return this.fornecedorForm.get('documento');
  }

  tipoFornecedorForm(): AbstractControl {
    return this.fornecedorForm.get('tipoFornecedor');
  }

  buscarCep(cep: string) {

    cep = StringUtils.somenteNumeros(cep);
    if (cep.length < 8) return;

    this.spinner.show();
    this.fornecedorService.consultarCep(cep)
      .subscribe({
        next: cepRetorno => { this.preencherEnderecoConsulta(cepRetorno); this.spinner.hide(); },
        error: erro => { this.errors.push(erro); this.spinner.hide(); }
      });
  }

  preencherEnderecoConsulta(cepConsulta: CepConsulta) {

    this.enderecoForm.patchValue({
      logradouro: cepConsulta.logradouro,
      bairro: cepConsulta.bairro,
      cep: cepConsulta.cep,
      cidade: cepConsulta.localidade,
      estado: cepConsulta.uf
    });
  }

  editarFornecedor() {
    if (this.fornecedorForm.dirty && this.fornecedorForm.valid) {

      this.fornecedor = Object.assign({}, this.fornecedor, this.fornecedorForm.value);
      this.fornecedor.documento = StringUtils.somenteNumeros(this.fornecedor.documento);

      /* Workaround para evitar cast de string para int no back-end */
      this.fornecedor.tipoFornecedor = parseInt(this.fornecedor.tipoFornecedor.toString());

      this.spinner.show();
      this.fornecedorService.atualizarFornecedor(this.fornecedor)
        .subscribe({
         next: sucesso => { this.spinner.hide(); this.processarSucesso(sucesso) },
         error: falha => { this.spinner.hide(); this.processarFalha(falha) }
        });
    }
  }

  processarSucesso(response: any) {
    this.errors = [];

    let toast = this.toastr.success('Fornecedor atualizado com sucesso!', 'Sucesso!');
    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/fornecedores/listar-todos']);
      });
    }
  }

  processarFalha(fail: any) {
    this.errors = fail.error.errors;
    this.toastr.error('Ocorreu um erro!', 'Opa :(');
    this.spinner.hide();
  }

  editarEndereco() {
    if (this.enderecoForm.dirty && this.enderecoForm.valid) {

      this.endereco = Object.assign({}, this.endereco, this.enderecoForm.value);

      this.endereco.cep = StringUtils.somenteNumeros(this.endereco.cep);
      this.endereco.fornecedorId = this.fornecedor.id;

      this.spinner.show();
      this.fornecedorService.atualizarEndereco(this.endereco)
        .subscribe({
         next: () => { this.spinner.hide(); this.processarSucessoEndereco(this.endereco) },
         error: falha => { this.spinner.hide(); this.processarFalhaEndereco(falha) }
        });
    }
  }

  processarSucessoEndereco(endereco: Endereco) {
    this.errors = [];

    this.toastr.success('Endereço atualizado com sucesso!', 'Sucesso!');
    this.fornecedor.endereco = endereco
    this.modalService.dismissAll();
  }

  processarFalhaEndereco(fail: any) {
    this.errorsEndereco = fail.error.errors;
    this.toastr.error('Ocorreu um erro!', 'Opa :(');
    this.spinner.hide();
  }

  abrirModal(content) {
    this.modalService.open(content);
  }
}
