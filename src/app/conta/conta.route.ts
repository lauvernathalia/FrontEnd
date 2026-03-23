import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContaAppComponent } from './conta.app.component';
import { CadastroComponent } from './cadastro/cadastro.component';
import { LoginComponent } from './login/login.component';
import { ContaGuard } from './services/conta.guard';
import { LogindashComponent } from './login/logindash.component';
import { DashboardComponent } from '../dashboard/home/dashboard.component';

const contaRouterConfig: Routes = [
    {
        path: '', component: LogindashComponent,//ContaAppComponent,
        children: [
            { path: 'cadastro', component: CadastroComponent, canActivate: [ContaGuard], canDeactivate: [ContaGuard] },
            { path: 'login', component: LoginComponent, canActivate: [ContaGuard] },
              { path: 'loginn', component: LogindashComponent, canActivate: [ContaGuard] },
              { path: 'Dashboard', component: DashboardComponent, canActivate: [ContaGuard] }
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