import { Component, Input } from '@angular/core';

type Trend = 'up' | 'down' | 'flat' | null;
type ActionIcon = 'download' | 'arrow-up';

@Component({
  selector: 'app-sales-card',
  template: `
    <article class="sales-card">
      <p class="sales-label">{{ title }}</p>

      <div class="sales-row">
        <div class="sales-value">
          <span class="sales-integer">{{ integerPart }}</span><span *ngIf="decimalPart" class="sales-decimal">.{{ decimalPart }}</span>
        </div>

        <div *ngIf="percent && trend" class="sales-percent" [ngClass]="percentClass">
          <svg *ngIf="trend === 'up'" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M12 19V5"></path><path d="m5 12 7-7 7 7"></path></svg>
          <svg *ngIf="trend === 'down'" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M12 5v14"></path><path d="m19 12-7 7-7-7"></path></svg>
          <svg *ngIf="trend === 'flat'" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M21 12a9 9 0 1 1-3-6.7"></path><path d="M21 3v5h-5"></path></svg>
          <span>{{ percent }}</span>
        </div>
      </div>

      <button type="button" class="sales-action">
        <span>{{ actionLabel }}</span>
        <svg *ngIf="actionIcon === 'download'" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M12 3v12"></path><path d="m7 10 5 5 5-5"></path><path d="M5 21h14"></path></svg>
        <svg *ngIf="actionIcon === 'arrow-up'" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M12 19V5"></path><path d="m5 12 7-7 7 7"></path></svg>
      </button>
    </article>
  `,
  styles: [
    `
      :host { display: block; }
      .sales-card { min-height: 158px; padding: 18px 20px 16px; border-radius: 20px; background: #ffffff; box-shadow: 0 14px 30px rgba(32, 56, 91, 0.08); border: 1px solid rgba(148, 163, 184, 0.16); display: flex; flex-direction: column; justify-content: space-between; }
      .sales-label { margin: 0; font-size: 13px; line-height: 1.3; font-weight: 600; color: #7e90a6; }
      .sales-row { display: flex; align-items: center; justify-content: space-between; gap: 14px; }
      .sales-value { display: flex; align-items: flex-end; color: #18324a; font-weight: 800; letter-spacing: -0.04em; }
      .sales-integer { font-size: 28px; line-height: 1; }
      .sales-decimal { font-size: 15px; line-height: 1; position: relative; top: -1px; }
      .sales-percent { display: inline-flex; align-items: center; gap: 7px; font-size: 13px; font-weight: 700; line-height: 1; white-space: nowrap; }
      .sales-percent svg { width: 14px; height: 14px; flex: 0 0 auto; }
      .sales-percent.up { color: #2eb55f; }
      .sales-percent.down { color: #ff5a86; }
      .sales-percent.flat { color: #2f9af4; }
      .sales-action { align-self: flex-start; border: 0; border-radius: 10px; background: #eef7e8; color: #51616f; padding: 9px 12px; font-size: 13px; font-weight: 600; display: inline-flex; align-items: center; gap: 10px; cursor: pointer; }
      .sales-action svg { width: 14px; height: 14px; color: #67b53d; flex: 0 0 auto; }
    `
  ]
})
export class SalesCardComponent {
  @Input() title = '';
  @Input() value = '';
  @Input() percent: string | null = null;
  @Input() trend: Trend = null;
  @Input() actionLabel = 'Extrato';
  @Input() actionIcon: ActionIcon = 'download';

  get integerPart(): string {
    return this.value.split(',')[0] ?? this.value;
  }

  get decimalPart(): string {
    return this.value.includes(',') ? (this.value.split(',')[1] ?? '') : '';
  }

  get percentClass(): string {
    return this.trend ?? '';
  }
}
