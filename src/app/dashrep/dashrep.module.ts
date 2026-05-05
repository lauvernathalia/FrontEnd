import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashrepAppComponent } from './dashrep.app.component';
import { DashrepRoutingModule } from './dashrep.route';
import { DashrepComponent } from './dashrep.component';
import { DashrepService } from './dashrep.service';

@NgModule({
  declarations: [
    DashrepAppComponent,
    DashrepComponent
  ],
  imports: [
    CommonModule,
    DashrepRoutingModule
  ],
  providers: [
    DashrepService
  ]
})
export class DashrepModule { }
