import { Component, OnInit, AfterViewInit, ViewChildren, ElementRef, inject, OnDestroy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, UntypedFormControl, FormControlName } from '@angular/forms';
import { Router } from '@angular/router';

import { CustomValidators } from '@narik/custom-validators';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Usuario } from '../../models/usuario';
import { ContaService } from '../../services/conta.service';

import { FormBaseComponent } from 'src/app/base-components/form-base.component';
import { MenuService } from 'src/app/navegacao/menu.service';

@Component({
  selector: 'app-cadastro-novo',
  templateUrl: './cadastroNovo.component.html',
  styleUrls: ['./cadastroNovo.component.css']
})
export class CadastroNovoComponent extends FormBaseComponent implements OnInit, AfterViewInit, OnDestroy {

   isSidebarCollapsed = false;
isCollapsed = false;

  toggleMenu() {
    this.isCollapsed = !this.isCollapsed;
  }
   toggleSidebar(): void {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }
 openedMenu: number | null = null;
  toggleSubmenu(index: number): void {
    this.openedMenu = this.openedMenu === index ? null : index;
  }

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  errors: any[] = [];
  cadastroForm: UntypedFormGroup;
  usuario: Usuario;
  // Dados mock para a lista/visualização da tela
  mocEstabelecimentos = [
    {
      id: 123456,
      nome: 'Maria das Dores da S. Costa',
      cnpj: '333.900.333/0001-11',
      cadastro: '09/09/2025',
      status: 'Habilitado',
      representante: 'PagueAgro',
      modalidade: 'Adquirencia',
      portal: false,
      payout: false,
      plataformas: ['Z','P','S','B']
    },
    {
      id: 123457,
      nome: 'Loja Central Ltda',
      cnpj: '111.222.333/0001-44',
      cadastro: '02/07/2024',
      status: 'Negado',
      representante: 'PagueAgro',
      modalidade: 'Adquirencia',
      portal: true,
      payout: false,
      plataformas: ['Z','P']
    },
    {
      id: 123458,
      nome: 'Padaria Bom Pão',
      cnpj: '555.666.777/0001-88',
      cadastro: '15/03/2023',
      status: 'Em análise',
      representante: 'PagueAgro',
      modalidade: 'Adquirencia',
      portal: true,
      payout: true,
      plataformas: ['Z','S']
    }
  ];

  private fb = inject(UntypedFormBuilder);
  private contaService = inject(ContaService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  private spinner = inject(NgxSpinnerService);
  private menuService = inject(MenuService);

  constructor() {
    super();

    this.validationMessages = {
      email: {
        required: 'Informe o e-mail',
        email: 'Email inválido'
      },
      password: {
        required: 'Informe a senha',
        rangeLength: 'A senha deve possuir entre 6 e 15 caracteres'
      },
      confirmPassword: {
        required: 'Informe a senha novamente',
        rangeLength: 'A senha deve possuir entre 6 e 15 caracteres',
        equalTo: 'As senhas não conferem'
      }
    };
    // mensagens para novos campos do cadastro
    this.validationMessages.parceiroNome = { required: 'Informe o nome do parceiro' };
    this.validationMessages.cnpj = { required: 'Informe o CNPJ' };
    this.validationMessages.cpf = { required: 'Informe o CPF' };
    this.validationMessages.razaoSocial = { required: 'Informe a razão social' };
    this.validationMessages.representante = { required: 'Informe o representante' };
    this.validationMessages.responsavelName = { required: 'Informe o nome do responsável' };
    this.validationMessages.responsavelCpf = { required: 'Informe o CPF do responsável' };
    this.validationMessages.responsavelEmail = { required: 'Informe o e-mail do responsável', email: 'E-mail inválido' };
    this.validationMessages.responsavelTelefone = { required: 'Informe o telefone do responsável' };

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {

    let senha = new UntypedFormControl('', [Validators.required, CustomValidators.rangeLength([6, 15])]);
    let senhaConfirm = new UntypedFormControl('', [Validators.required, CustomValidators.rangeLength([6, 15]), CustomValidators.equalTo(senha)]);

    this.cadastroForm = this.fb.group({
      parceiroNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: senha,
      confirmPassword: senhaConfirm,
      documentType: ['cnpj'],
      cnpj: [''],
      cpf: [''],
      razaoSocial: [''],
      representante: [''],
      modalidade: [''],
      telefone: [''],
      responsavelName: [''],
      responsavelCpf: [''],
      responsavelCargo: [''],
      responsavelTelefone: [''],
      responsavelEmail: ['']
    });

    // Ajuste dinâmico de validação entre CPF/CNPJ
    this.cadastroForm.get('documentType')?.valueChanges.subscribe(val => {
      const cnpjCtrl = this.cadastroForm.get('cnpj');
      const cpfCtrl = this.cadastroForm.get('cpf');
      if (val === 'cnpj') {
        cnpjCtrl?.setValidators([Validators.required]);
        cpfCtrl?.clearValidators();
        cpfCtrl?.setValue('');
      } else {
        cpfCtrl?.setValidators([Validators.required]);
        cnpjCtrl?.clearValidators();
        cnpjCtrl?.setValue('');
      }
      cnpjCtrl?.updateValueAndValidity();
      cpfCtrl?.updateValueAndValidity();
    });

    // Validações para responsável
    this.cadastroForm.get('responsavelName')?.setValidators([Validators.required]);
    this.cadastroForm.get('responsavelEmail')?.setValidators([Validators.required, Validators.email]);
    this.cadastroForm.get('responsavelTelefone')?.setValidators([Validators.required]);

    // Hide menu while on this registration form
    this.menuService.setOverride(false);
  }

  // Tabs
  activeTab: number = 1;

  setActiveTab(tab: number) {
    if (tab < 1) tab = 1;
    if (tab > 6) tab = 6;
    this.activeTab = tab;
  }

  nextTab() {
    this.setActiveTab(this.activeTab + 1);
  }

  prevTab() {
    this.setActiveTab(this.activeTab - 1);
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.cadastroForm);
  }

  ngOnDestroy(): void {
    // Remove explicit override so menu visibility falls back to router logic
    this.menuService.setOverride(null);
  }

  adicionarConta() {
    if (this.cadastroForm.dirty && this.cadastroForm.valid) {
      this.usuario = Object.assign({}, this.usuario, this.cadastroForm.value);

      this.spinner.show();
      this.contaService.registrarUsuario(this.usuario)
        .subscribe(
          sucesso => { this.spinner.hide(); this.processarSucesso(sucesso) },
          falha => { this.spinner.hide(); this.processarFalha(falha) }
        );

      this.mudancasNaoSalvas = false;
    }
  }

  onCpfInput(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input) return;
    let val = input.value.replace(/\D/g, '').slice(0, 11);
    let masked = val;
    if (val.length > 9) {
      masked = val.replace(/^(\d{3})(\d{3})(\d{3})(\d{0,2}).*/, '$1.$2.$3-$4');
    } else if (val.length > 6) {
      masked = val.replace(/^(\d{3})(\d{3})(\d{0,3}).*/, '$1.$2.$3');
    } else if (val.length > 3) {
      masked = val.replace(/^(\d{3})(\d{0,3}).*/, '$1.$2');
    }
    input.value = masked;
    const ctrl = this.cadastroForm.get('responsavelCpf');
    if (ctrl) ctrl.setValue(masked, { emitEvent: false });
  }

  processarSucesso(response: any) {
    this.cadastroForm.reset();
    this.errors = [];

    this.contaService.LocalStorage.salvarDadosLocaisUsuario(response);

    let toast = this.toastr.success('Registro realizado com Sucesso!', 'Bem vindo!!!');
    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/home']);
      });
    }
  }

  processarFalha(fail: any) {
    this.errors = fail.error.errors;
    this.toastr.error('Ocorreu um erro!', 'Opa :(');
    this.spinner.hide();
  }
}
