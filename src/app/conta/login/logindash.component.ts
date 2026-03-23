import { Component, OnInit, OnDestroy, ViewChildren, ElementRef, inject } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormControlName } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { CustomValidators } from '@narik/custom-validators';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Usuario } from '../models/usuario';
import { ContaService } from '../services/conta.service';
import { FormBaseComponent } from 'src/app/base-components/form-base.component';


@Component({
  selector: 'app-login',
  templateUrl: './logindash.component.html'
})
export class LogindashComponent extends FormBaseComponent implements OnInit {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[];

  errors: any[] = [];
  loginForm: UntypedFormGroup;
  usuario: Usuario;

  returnUrl: string;

  showEmailLogin: boolean = false;
  qrImageUrl: string = 'assets/qr-placeholder.svg';
  qrExpiresIn: number = 60; // seconds
  qrTimerDisplay: string = '01:00';
  private qrInterval: any;

  private fb = inject(UntypedFormBuilder);
  private contaService = inject(ContaService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private toastr = inject(ToastrService);
  private spinner = inject(NgxSpinnerService);

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
      }
    };

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }

  ngOnInit(): void {

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, CustomValidators.rangeLength([6, 15])]]
    });

    this.startQrTimer();
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.loginForm);
  }

  login() {
    if (this.loginForm.dirty && this.loginForm.valid) {
      this.usuario = Object.assign({}, this.usuario, this.loginForm.value);

      this.spinner.show();
      // this.contaService.login(this.usuario)
      // .subscribe(
      //     sucesso => { this.spinner.hide(); this.processarSucesso(sucesso) },
      //     falha => { this.spinner.hide(); this.processarFalha(falha) }
      // );
      this.spinner.hide(); this.processarSucesso(this.usuario)
    }
  }

  toggleEmailLogin() {
    this.showEmailLogin = !this.showEmailLogin;

  }

  private startQrTimer() {
    this.qrExpiresIn = 60;
    this.updateQrDisplay();
    this.qrInterval = setInterval(() => {
      this.qrExpiresIn--;
      if (this.qrExpiresIn <= 0) {
        clearInterval(this.qrInterval);
        this.qrTimerDisplay = '00:00';
        return;
      }
      this.updateQrDisplay();
    }, 1000);
  }

  private updateQrDisplay() {
    const mm = Math.floor(this.qrExpiresIn / 60).toString().padStart(2, '0');
    const ss = (this.qrExpiresIn % 60).toString().padStart(2, '0');
    this.qrTimerDisplay = `${mm}:${ss}`;
  }

  ngOnDestroy(): void {
    if (this.qrInterval) {
      clearInterval(this.qrInterval);
    }
  }

  createAccount() {
    this.router.navigate(['/conta/cadastro']);
  }

  refreshQr() {
    // placeholder: in a real app you would request a new QR payload from backend
    this.qrImageUrl = 'assets/qr-placeholder.png';
    if (this.qrInterval) { clearInterval(this.qrInterval); }
    this.startQrTimer();
  }

  processarSucesso(response: any) {
    this.loginForm.reset();
    this.errors = [];

    //this.contaService.LocalStorage.salvarDadosLocaisUsuario(response);

    let toast = this.toastr.success('Login realizado com Sucesso!', 'Bem vindo!!!');
    if(toast){
      toast.onHidden.subscribe(() => {
        this.returnUrl
        ? this.router.navigate([this.returnUrl])
        : this.router.navigate(['/dashboard']);
      });
    }
  }

  processarFalha(fail: any){
    this.errors = fail.error.errors;
    this.toastr.error('Ocorreu um erro!', 'Opa :(');
    this.spinner.hide();
  }
}
