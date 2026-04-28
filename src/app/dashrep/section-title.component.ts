import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-section-title',
  template: `
    <div class="section-title-row">
      <h3 class="section-title">{{ title }}</h3>
      <span class="section-title-line" aria-hidden="true"></span>
    </div>
  `,
  styles: [
    `
      :host { display: block; }
      .section-title-row { display: flex; align-items: center; gap: 18px; margin-bottom: 20px; }
      .section-title { margin: 0; font-size: 16px; line-height: 1; font-weight: 700; color: #8595a8; }
      .section-title-line { width: 92px; height: 2px; border-radius: 999px; background: linear-gradient(90deg, #c9e7c8 0%, #8fca94 100%); }
    `
  ]
})
export class SectionTitleComponent {
  @Input() title = '';
}
