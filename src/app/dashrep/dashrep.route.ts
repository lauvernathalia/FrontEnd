import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashrepAppComponent } from './dashrep.app.component';
import { DashrepComponent } from './dashrep.component';

const dashrepRoutes: Routes = [
  {
    path: '',
    component: DashrepAppComponent,
    children: [
      { path: '', component: DashrepComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(dashrepRoutes)],
  exports: [RouterModule]
})
export class DashrepRoutingModule { }
