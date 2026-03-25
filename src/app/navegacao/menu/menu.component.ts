import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription, filter } from 'rxjs';
import { MenuService } from '../menu.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html'
})
export class MenuComponent implements OnInit, OnDestroy {

  public isCollapsed: boolean = true;
  public showMenu: boolean = true;

  private routerSub: Subscription;
  private overrideSub: Subscription;
  private baseShow: boolean = true; // computed from router
  private lastOverride: boolean | null = null; // override from service

  constructor(private router: Router, private menuService: MenuService) { }

  ngOnInit(): void {
    this.routerSub = this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe((ev: NavigationEnd) => {
        const url = ev.urlAfterRedirects || ev.url;
        this.baseShow = !url.startsWith('/dashboard');
        this.recomputeShow();
      });

    this.overrideSub = this.menuService.override$.subscribe((ov) => {
      this.lastOverride = ov;
      this.recomputeShow();
    });
  }

  ngOnDestroy(): void {
    if (this.routerSub) { this.routerSub.unsubscribe(); }
    if (this.overrideSub) { this.overrideSub.unsubscribe(); }
  }

  private recomputeShow(): void {
    this.showMenu = this.lastOverride !== null ? this.lastOverride : this.baseShow;
  }

  public toggle(): void {
    this.isCollapsed = !this.isCollapsed;
  }
}
