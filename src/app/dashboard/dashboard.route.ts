import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardAppComponent } from './dashboard.app.component';
import { DashboardComponent } from './home/dashboard.component';

const dashboardRoutes: Routes = [
  {
    path: '',
    component: DashboardAppComponent,
    children: [
      { path: '', component: DashboardComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(dashboardRoutes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
