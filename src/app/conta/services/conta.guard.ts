import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CadastroComponent } from '../cadastro/cadastro.component';
import { CadastroestComponent } from '../cadastro/cadastroest.component';
import { LocalStorageUtils } from 'src/app/utils/localstorage';


@Injectable()
export class ContaGuard  {
    
    localStorageUtils = new LocalStorageUtils();

    private router = inject(Router);
    
    canDeactivate(component: CadastroComponent | CadastroestComponent) {
        if(component.mudancasNaoSalvas) {
            return window.confirm('Tem certeza que deseja abandonar o preenchimento do formulario ?');
        }  

        return true
    }

    canActivate() {
        let tk = this.localStorageUtils.obterTokenUsuario();
        if(this.localStorageUtils.obterTokenUsuario()){
            this.router.navigate(['/home']);
        }

        return true;
    }
    
}