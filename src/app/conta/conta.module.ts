import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { CadastroComponent } from './cadastro/cadastro.component';
import { LoginComponent } from './login/login.component';
import { LogindashComponent } from './login/logindash.component';
import { ContaAppComponent } from './conta.app.component';
import { DashboardAppComponent } from '../dashboard/dashboard.app.component';
import { ContaRoutingModule } from './conta.route';
import { ContaService } from './services/conta.service';

import { NarikCustomValidatorsModule } from '@narik/custom-validators';
import { ContaGuard } from './services/conta.guard';
import { CadastroestComponent } from './cadastro/cadastroest.component';
import { CadastroNovoComponent } from './cadastro/Novo/cadastroNovo.component';

@NgModule({
  declarations: [
    ContaAppComponent,
    CadastroComponent, 
    CadastroestComponent,
    LoginComponent, 
    LogindashComponent,
    CadastroNovoComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ContaRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NarikCustomValidatorsModule
  ],
  providers: [
    ContaService,
    ContaGuard
  ]
})
export class ContaModule { }
