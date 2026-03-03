import { Component } from '@angular/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html'
})
export class MenuComponent {

  public isCollapsed: boolean = true;

  constructor() {
  }

  public toggle(): void {
    this.isCollapsed = !this.isCollapsed;
  }
}
