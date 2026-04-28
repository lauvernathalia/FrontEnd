import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashrepAppComponent } from './dashrep.app.component';
import { DashrepRoutingModule } from './dashrep.route';
import { DashrepComponent } from './dashrep.component';
import { DashrepService } from './dashrep.service';
import { SectionTitleComponent } from './section-title.component';
import { SalesCardComponent } from './sales-card.component';

@NgModule({
  declarations: [
    DashrepAppComponent,
    DashrepComponent,
    SectionTitleComponent,
    SalesCardComponent
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
