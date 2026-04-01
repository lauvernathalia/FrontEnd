import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContaAppComponent } from './conta.app.component';
import { CadastroComponent } from './cadastro/cadastro.component';
import { LoginComponent } from './login/login.component';
import { ContaGuard } from './services/conta.guard';
import { LogindashComponent } from './login/logindash.component';
import { DashboardComponent } from '../dashboard/home/dashboard.component';
import { CadastroestComponent } from './cadastro/cadastroest.component';

const contaRouterConfig: Routes = [
    {
        path: '', component:ContaAppComponent, //LogindashComponent,//ContaAppComponent,
        children: [
            { path: 'cadastro', component: CadastroComponent, canActivate: [ContaGuard] },
            { path: 'login', component: LoginComponent, canActivate: [ContaGuard] },
              { path: 'loginn', component: LogindashComponent, canActivate: [ContaGuard] },
              { path: 'Dashboard', component: DashboardComponent, canActivate: [ContaGuard] },
                  { path: 'cadastroest', component: CadastroestComponent}
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(contaRouterConfig)
    ],
    exports: [RouterModule]
})
export class ContaRoutingModule { }