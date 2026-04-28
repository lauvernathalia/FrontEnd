import { Component, Input } from '@angular/core';

export type PaymentSegment = {
  label: string;
  percent: number;
  color: string;
};

@Component({
  selector: 'app-payment-donut',
  template: `
    <div class="payment-donut-layout">
      <ul class="payment-legend" aria-label="Legenda das formas de pagamento">
        <li *ngFor="let segment of segments" class="payment-legend-item">
          <span class="legend-dot" [style.backgroundColor]="segment.color" aria-hidden="true"></span>
          <span>{{ segment.label }}</span>
        </li>
      </ul>

      <div class="donut-stage" aria-label="Gráfico donut de formas de pagamento">
        <div class="donut-chart" [style.background]="conicGradient" role="img" [attr.aria-label]="ariaLabel"></div>
        <div class="donut-hole"></div>
        <div class="donut-badge badge-credit"><span>Crédito</span><strong>60%</strong></div>
        <div class="donut-badge badge-debit"><span>Débito</span><strong>25%</strong></div>
      </div>
    </div>
  `,
  styles: [
    `
      :host { display: block; }
      .payment-donut-layout { display: grid; grid-template-columns: 140px minmax(0, 1fr); gap: 18px; align-items: center; min-height: 300px; }
      .payment-legend { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 18px; }
      .payment-legend-item { display: flex; align-items: center; gap: 12px; color: #8192a4; font-size: 15px; font-weight: 500; }
      .legend-dot { width: 16px; height: 16px; border-radius: 999px; flex: 0 0 auto; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08) inset; }
      .donut-stage { position: relative; width: 100%; min-height: 300px; display: flex; align-items: center; justify-content: center; }
      .donut-chart { width: 240px; height: 240px; border-radius: 50%; box-shadow: inset 0 0 0 8px rgba(215, 222, 228, 0.95), 0 14px 28px rgba(34, 49, 63, 0.08); }
      .donut-hole { position: absolute; width: 102px; height: 102px; border-radius: 50%; background: #fff; box-shadow: 0 0 0 8px #f7f9fb; }
      .donut-badge { position: absolute; z-index: 2; display: inline-flex; flex-direction: column; gap: 2px; padding: 8px 10px; border-radius: 12px; background: #fff; box-shadow: 0 8px 20px rgba(34, 49, 63, 0.08); border: 1px solid rgba(148, 163, 184, 0.14); font-size: 11px; color: #a3afba; font-weight: 600; line-height: 1; }
      .donut-badge strong { font-size: 14px; color: #5e8a31; }
      .donut-badge::after { content: ''; position: absolute; width: 10px; height: 10px; background: inherit; border-left: 1px solid rgba(148, 163, 184, 0.14); border-bottom: 1px solid rgba(148, 163, 184, 0.14); transform: rotate(45deg); }
      .badge-credit { top: 62px; left: 92px; }
      .badge-credit::after { left: -5px; bottom: 16px; }
      .badge-debit { top: 68px; right: 94px; }
      .badge-debit::after { right: -5px; bottom: 16px; }
      @media (max-width: 1280px) { .payment-donut-layout { grid-template-columns: 120px minmax(0, 1fr); } .donut-chart { width: 220px; height: 220px; } }
      @media (max-width: 640px) { .payment-donut-layout { grid-template-columns: 1fr; gap: 20px; min-height: auto; } .payment-legend { flex-direction: row; flex-wrap: wrap; gap: 12px 18px; } .donut-stage { min-height: 280px; } .badge-credit { left: 34px; } .badge-debit { right: 34px; } }
    `
  ]
})
export class PaymentDonutComponent {
  @Input() segments: PaymentSegment[] = [];

  get conicGradient(): string {
    const fallbackColors = ['#355d1a', '#6ea93e', '#a7d785', '#d5ebc0'];
    const colors = this.segments.length ? this.segments.map((segment) => segment.color) : fallbackColors;
    const sizes = this.segments.length ? this.segments.map((segment) => segment.percent) : [60, 25, 10, 5];
    const total = sizes.reduce((sum, value) => sum + value, 0) || 100;

    let accumulated = 0;
    const stops = sizes.map((size, index) => {
      const start = (accumulated / total) * 100;
      accumulated += size;
      const end = (accumulated / total) * 100;
      return `${colors[index % colors.length]} ${start}% ${end}%`;
    });

    return `conic-gradient(${stops.join(', ')})`;
  }

  get ariaLabel(): string {
    return this.segments.map((segment) => `${segment.label} ${segment.percent}%`).join(', ');
  }
}
