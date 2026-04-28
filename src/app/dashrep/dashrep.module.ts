import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DashrepAppComponent } from './dashrep.app.component';
import { DashrepRoutingModule } from './dashrep.route';
import { DashrepComponent } from './dashrep.component';
import { DashrepService } from './dashrep.service';
import { SectionTitleComponent } from './section-title.component';
import { SalesCardComponent } from './sales-card.component';
import { PaymentDonutComponent } from './payment-donut.component';
import { BrandItemComponent } from './brand-item.component';
import { PaymentMethodsSectionComponent } from './payment-methods-section.component';

@NgModule({
  declarations: [
    DashrepAppComponent,
    DashrepComponent,
    SectionTitleComponent,
    SalesCardComponent,
    PaymentDonutComponent,
    BrandItemComponent,
    PaymentMethodsSectionComponent
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
