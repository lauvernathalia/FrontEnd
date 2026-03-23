import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription, filter } from 'rxjs';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html'
})
export class MenuComponent implements OnInit, OnDestroy {

  public isCollapsed: boolean = true;
  public showMenu: boolean = true;

  private routerSub: Subscription;

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.routerSub = this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe((ev: NavigationEnd) => {
        // Hide menu on dashboard routes (adjust condition as needed)
        const url = ev.urlAfterRedirects || ev.url;
        this.showMenu = !url.startsWith('/dashboard');
      });
  }

  ngOnDestroy(): void {
    if (this.routerSub) { this.routerSub.unsubscribe(); }
  }

  public toggle(): void {
    this.isCollapsed = !this.isCollapsed;
  }
}
