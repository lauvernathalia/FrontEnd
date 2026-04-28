import { Component } from '@angular/core';
import { PaymentSegment } from './payment-donut.component';

@Component({
  selector: 'app-payment-methods-section',
  template: `
    <section class="payment-methods-section" aria-label="Formas de pagamento e por bandeira">
      <div class="payment-grid">
        <article class="payment-card">
          <app-section-title title="Formas de pagamento"></app-section-title>
          <app-payment-donut [segments]="paymentSegments"></app-payment-donut>
        </article>

        <article class="payment-card">
          <app-section-title title="Por bandeira"></app-section-title>
          <div class="brand-subtitle">
            <span class="brand-subtitle-icon"><i class="fas fa-flag" aria-hidden="true"></i></span>
            <span>Vendas por bandeira (R$)</span>
          </div>
          <div class="brand-list">
            <app-brand-item *ngFor="let brand of brands" [name]="brand.name" [value]="brand.value" [brandKey]="brand.brandKey"></app-brand-item>
          </div>
        </article>
      </div>
    </section>
  `,
  styles: [
    `
      :host { display: block; }
      .payment-methods-section { margin-top: 20px; margin-bottom: 8px; }
      .payment-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 18px; align-items: start; }
      .payment-card { background: #ffffff; border-radius: 24px; padding: 22px 22px 20px; box-shadow: 0 12px 28px rgba(34, 49, 63, 0.06); border: 1px solid rgba(34, 34, 34, 0.03); min-height: 382px; }
      .brand-subtitle { display: inline-flex; align-items: center; gap: 10px; color: #7e90a6; font-size: 14px; font-weight: 700; margin: 6px 0 18px 2px; }
      .brand-subtitle-icon { width: 26px; height: 26px; border-radius: 999px; background: #f2f9ea; color: #78af39; display: inline-flex; align-items: center; justify-content: center; font-size: 12px; }
      .brand-list { display: flex; flex-direction: column; gap: 20px; padding-top: 6px; }
      @media (max-width: 900px) { .payment-grid { grid-template-columns: 1fr; } .payment-card { min-height: auto; } }
      @media (max-width: 640px) { .payment-card { padding: 18px 16px 16px; border-radius: 20px; } }
    `
  ]
})
export class PaymentMethodsSectionComponent {
  paymentSegments: PaymentSegment[] = [
    { label: 'Crédito', percent: 60, color: '#355d1a' },
    { label: 'Débito', percent: 25, color: '#6ea93e' },
    { label: 'Boleto', percent: 10, color: '#a7d785' },
    { label: 'Pix', percent: 5, color: '#d5ebc0' }
  ];

  brands = [
    { name: 'Mastercard', value: 'R$ 9.678,89', brandKey: 'mastercard' as const },
    { name: 'Visa', value: 'R$ 9.678,89', brandKey: 'visa' as const },
    { name: 'Hipercard', value: 'R$ 9.678,89', brandKey: 'hipercard' as const },
    { name: 'Outras', value: 'R$ 9.678,89', brandKey: 'other' as const }
  ];
}
