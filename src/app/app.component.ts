import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'front-end';
  exibirNavegacao = true;

  constructor(private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const url = event.urlAfterRedirects || event.url;
        const hiddenRoutes = ['/dashboard', '/conta/loginn', '/conta/cadastronovo'];
        this.exibirNavegacao = !hiddenRoutes.some(r => url.startsWith(r));
      }
    });
  }
}
