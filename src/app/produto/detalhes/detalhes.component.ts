import { Component, inject } from '@angular/core';
import { Produto } from '../models/produto';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-detalhes',
  templateUrl: './detalhes.component.html'
})
export class DetalhesComponent {

  private route = inject(ActivatedRoute);

  imagens: string = environment.imagensUrl;
  produto: Produto = this.route.snapshot.data['produto'];

}
