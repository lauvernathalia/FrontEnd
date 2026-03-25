import { Component, OnInit, AfterViewInit, ViewChildren, ElementRef, inject, OnDestroy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, UntypedFormControl, FormControlName } from '@angular/forms';
import { Router } from '@angular/router';

import { CustomValidators } from '@narik/custom-validators';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Usuario } from '../models/usuario';
import { ContaService } from '../services/conta.service';

import { FormBaseComponent } from 'src/app/base-components/form-base.component';
import { MenuService } from 'src/app/navegacao/menu.service';

@Component({
  selector: 'app-cadastroest',
  templateUrl: './cadastroest.component.html',
  styleUrls: ['./cadastroest.component.css']
})
export class CadastroestComponent extends FormBaseComponent implements OnInit, AfterViewInit, OnDestroy {

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

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {

    let senha = new UntypedFormControl('', [Validators.required, CustomValidators.rangeLength([6, 15])]);
    let senhaConfirm = new UntypedFormControl('', [Validators.required, CustomValidators.rangeLength([6, 15]), CustomValidators.equalTo(senha)]);

    this.cadastroForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: senha,
      confirmPassword: senhaConfirm
    });

    // Hide menu while on this registration form
    this.menuService.setOverride(false);
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
