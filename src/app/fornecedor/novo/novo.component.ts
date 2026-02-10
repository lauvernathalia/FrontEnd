import { Component, OnInit, ViewChildren, ElementRef } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormControlName, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Fornecedor } from '../models/fornecedor';
import { FornecedorService } from '../services/fornecedor.service';
import { CepConsulta } from '../models/endereco';
import { StringUtils } from 'src/app/utils/string-utils';
import { FormBaseComponent } from 'src/app/base-components/form-base.component';

@Component({
  selector: 'app-novo',
  templateUrl: './novo.component.html'
})
export class NovoComponent extends FormBaseComponent implements OnInit {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  errors: any[] = [];
  fornecedorForm: UntypedFormGroup;
  fornecedor: Fornecedor = new Fornecedor();

  textoDocumento: string = 'CPF (requerido)';
  // Máscara combinada CPF/CNPJ: o ngx-mask escolhe o padrão conforme o tamanho
  documentoMask: string = '000.000.000-00|00.000.000/0000-00';
  formResult: string = '';
  
  constructor(private fb: UntypedFormBuilder,
    private fornecedorService: FornecedorService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) {

    super();

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
        cep: 'CEP em formato inválido'
      },
      cidade: {
        required: 'Informe a Cidade',
      },
      estado: {
        required: 'Informe o Estado',
      }
    };

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit() {

    this.fornecedorForm = this.fb.group({
      nome: ['', [Validators.required]],
      documento: ['', [Validators.required]],
      ativo: ['', [Validators.required]],
      tipoFornecedor: ['', [Validators.required]],

      endereco: this.fb.group({
        logradouro: ['', [Validators.required]],
        numero: ['', [Validators.required]],
        complemento: [''],
        bairro: ['', [Validators.required]],
        cep: ['', [Validators.required]],
        cidade: ['', [Validators.required]],
        estado: ['', [Validators.required]]
      })
    });

    this.fornecedorForm.patchValue({ tipoFornecedor: '1', ativo: true });
  }

  ngAfterViewInit(): void {

    this.tipoFornecedorForm().valueChanges
      .subscribe(() => {
        this.trocarValidacaoDocumento();
        super.configurarValidacaoFormularioBase(this.formInputElements, this.fornecedorForm)
        super.validarFormulario(this.fornecedorForm);
      });

    this.documento().valueChanges
      .subscribe((value: string) => {
        this.ajustarTipoFornecedorPorDocumento(value);
      });

    super.configurarValidacaoFormularioBase(this.formInputElements, this.fornecedorForm)
  }
    
  trocarValidacaoDocumento() {
    if (this.tipoFornecedorForm().value === "1") {
      this.documento().clearValidators();
      this.documento().setValidators([Validators.required]);
      this.textoDocumento = "CPF (requerido)";
    }
    else {
      this.documento().clearValidators();
      this.documento().setValidators([Validators.required]);
      this.textoDocumento = "CNPJ (requerido)";
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

  tipoFornecedorForm(): AbstractControl {
    return this.fornecedorForm.get('tipoFornecedor');
  }

  documento(): AbstractControl {
    return this.fornecedorForm.get('documento');
  }

  buscarCep(cep: string) {

    cep = StringUtils.somenteNumeros(cep);
    if (cep.length < 8) return;

    this.spinner.show();
    this.fornecedorService.consultarCep(cep)
      .subscribe(
        cepRetorno => { this.preencherEnderecoConsulta(cepRetorno); this.spinner.hide(); },
        erro => { this.errors.push(erro); this.spinner.hide(); });
  }

  preencherEnderecoConsulta(cepConsulta: CepConsulta) {

    this.fornecedorForm.patchValue({
      endereco: {
        logradouro: cepConsulta.logradouro,
        bairro: cepConsulta.bairro,
        cep: cepConsulta.cep,
        cidade: cepConsulta.localidade,
        estado: cepConsulta.uf
      }
    });
  }

  adicionarFornecedor() {
    if (this.fornecedorForm.dirty && this.fornecedorForm.valid) {

      this.fornecedor = Object.assign({}, this.fornecedor, this.fornecedorForm.value);
      this.formResult = JSON.stringify(this.fornecedor);

      this.fornecedor.endereco.cep = StringUtils.somenteNumeros(this.fornecedor.endereco.cep);
      this.fornecedor.documento = StringUtils.somenteNumeros(this.fornecedor.documento);
      // forçando o tipo fornecedor ser serializado como INT
      this.fornecedor.tipoFornecedor = parseInt(this.fornecedor.tipoFornecedor.toString());

      this.spinner.show();
      this.fornecedorService.novoFornecedor(this.fornecedor)
        .subscribe(
          sucesso => { this.spinner.hide(); this.processarSucesso(sucesso) },
          falha => { this.spinner.hide(); this.processarFalha(falha) }
        );
    }
  }

  processarSucesso(response: any) {
    this.fornecedorForm.reset();
    this.errors = [];

    this.mudancasNaoSalvas = false;

    let toast = this.toastr.success('Fornecedor cadastrado com sucesso!', 'Sucesso!');
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
}