import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CadastroComponent } from '../cadastro/cadastro.component';
import { LocalStorageUtils } from 'src/app/utils/localstorage';


@Injectable()
export class ContaGuard  {
    
    localStorageUtils = new LocalStorageUtils();

    private router = inject(Router);
    
    canDeactivate(component: CadastroComponent) {
        if(component.mudancasNaoSalvas) {
            return window.confirm('Tem certeza que deseja abandonar o preenchimento do formulario?');
        }  

        return true
    }

    canActivate() {
        if(this.localStorageUtils.obterTokenUsuario()){
            this.router.navigate(['/home']);
        }

        return true;
    }
    
}