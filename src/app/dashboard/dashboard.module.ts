import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashboardAppComponent } from './dashboard.app.component';
import { DashboardRoutingModule } from './dashboard.route';
import { DashboardComponent } from './home/dashboard.component';
import { DashboardService } from './services/dashboard.service';

@NgModule({
  declarations: [
    DashboardAppComponent,
    DashboardComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule
  ],
  providers: [
    DashboardService
  ]
})
export class DashboardModule { }
