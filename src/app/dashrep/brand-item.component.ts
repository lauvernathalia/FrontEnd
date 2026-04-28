import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-brand-item',
  template: `
    <div class="brand-item">
      <div class="brand-icon" aria-hidden="true">
        <ng-container [ngSwitch]="brandKey">
          <span *ngSwitchCase="'mastercard'" class="mastercard-mark"><span class="circle circle-left"></span><span class="circle circle-right"></span></span>
          <span *ngSwitchCase="'visa'" class="visa-mark">VISA</span>
          <span *ngSwitchCase="'hipercard'" class="hipercard-mark">H</span>
          <span *ngSwitchDefault class="other-mark"><i class="fas fa-credit-card"></i></span>
        </ng-container>
      </div>

      <div class="brand-name">{{ name }}</div>
      <div class="brand-dots" aria-hidden="true"></div>
      <div class="brand-value">{{ value }}</div>
    </div>
  `,
  styles: [
    `
      :host { display: block; }
      .brand-item { display: grid; grid-template-columns: 56px minmax(0, auto) minmax(0, 1fr) auto; align-items: center; gap: 14px; min-height: 58px; }
      .brand-icon { width: 48px; height: 48px; border-radius: 14px; background: #f8fbf3; border: 1px solid #dcecc8; display: flex; align-items: center; justify-content: center; color: #6a8f31; flex: 0 0 auto; }
      .mastercard-mark { position: relative; width: 28px; height: 18px; display: block; }
      .circle { position: absolute; top: 0; width: 18px; height: 18px; border-radius: 50%; }
      .circle-left { left: 0; background: #ff5f3a; }
      .circle-right { right: 0; background: #f7b21b; }
      .visa-mark { font-size: 14px; font-weight: 800; color: #1455d8; letter-spacing: 0.02em; }
      .hipercard-mark { font-size: 14px; font-weight: 800; color: #cc293d; }
      .other-mark i { font-size: 16px; color: #78af39; }
      .brand-name { font-size: 14px; font-weight: 700; color: #7f90a2; letter-spacing: 0.02em; text-transform: uppercase; }
      .brand-dots { border-bottom: 2px dotted rgba(176, 188, 200, 0.6); transform: translateY(1px); }
      .brand-value { font-size: 15px; font-weight: 700; color: #6bbb34; white-space: nowrap; }
      @media (max-width: 640px) { .brand-item { grid-template-columns: 48px minmax(0, auto) minmax(0, 1fr) auto; gap: 12px; } .brand-value { font-size: 14px; } }
    `
  ]
})
export class BrandItemComponent {
  @Input() name = '';
  @Input() value = '';
  @Input() brandKey: 'mastercard' | 'visa' | 'hipercard' | 'other' = 'other';
}
