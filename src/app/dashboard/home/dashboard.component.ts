import { Component, AfterViewInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.componentcopy.html',
  styleUrls: ['./dashboard.componentcopy.css']
})
/**
 * DashboardComponent
 * ------------------
 * Este componente renderiza a página do dashboard e, entre outros blocos
 * de UI, gera as linhas SVG do gráfico "Progressão das vendas".
 *
 * Resumo do fluxo (como as linhas SVG são populadas):
 * 1. Duas arrays públicas `lightValues` e `darkValues` contêm 12 valores
 *    numéricos (um por mês). Estas arrays estão declaradas no final do arquivo.
 * 2. Em `ngAfterViewInit()` chamamos `updateSalesSvg()` uma vez e anexamos
 *    um listener em `resize` para recalcular o SVG quando a janela mudar
 *    de tamanho.
 * 3. `updateSalesSvg()` mapeia os valores numéricos para coordenadas x/y
 *    dentro do `viewBox` do SVG, converte os pontos em um caminho suave
 *    (Catmull–Rom → Bézier) e atribui a string `d` às duas tags `<path>`
 *    (`.line-light` e `.line-dark`).
 * 4. Para alterar manualmente as linhas, edite as arrays `lightValues` e
 *    `darkValues` (cada uma deve conter 12 números); o componente irá
 *    regenerar os paths no próximo render ou redimensionamento.
 */
export class DashboardComponent implements AfterViewInit, OnDestroy {

  isSidebarCollapsed = false;
isCollapsed = false;

  toggleMenu() {
    this.isCollapsed = !this.isCollapsed;
  }
  menus = {
    operacao: true,
    solicitacoes: true,
    cadastros: true
  };

  toggleSidebar(): void {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  toggleSubmenu(menu: 'operacao' | 'solicitacoes' | 'cadastros'): void {
    this.menus[menu] = !this.menus[menu];
  }
  
  ngAfterViewInit(): void {
    // render inicial: preenche os paths SVG usando as arrays abaixo
    this.updateSalesSvg();
    // atualiza no resize para manter as coordenadas corretas quando o
    // tamanho do gráfico mudar
    window.addEventListener('resize', this._resizeHandler);
  }

  ngOnDestroy(): void {
    window.removeEventListener('resize', this._resizeHandler);
  }

  private _resizeHandler = () => { this.updateSalesSvg(); };

  /**
   * updateSalesSvg
   * - Lê as arrays `lightValues` e `darkValues`
   * - Mapeia os números para coordenadas SVG (x,y) dentro do `viewBox`
   * - Produz uma string de path suave usando Catmull–Rom -> Bézier
   * - Atribui a string `d` às paths `.line-light` e `.line-dark` dentro de
   *   `.sales-svg`.
   * Observações:
   * - O mapeamento usa o máximo da array para escalar verticalmente os
   *   valores (as curvas ocupam a altura disponível proporcionalmente).
   * - Se preferir uma escala vertical fixa (ex.: max = 100000), substitua
   *   o `max` calculado abaixo por um valor constante.
   */
  private updateSalesSvg(): void {
    try {
      const svg = document.querySelector('.sales-svg') as SVGElement;
      if (!svg) return;

      const light = (this.lightValues && this.lightValues.length) ? this.lightValues : [];
      const dark = (this.darkValues && this.darkValues.length) ? this.darkValues : [];
      if (!light.length || !dark.length) return;

      const xStart = 80, xEnd = 680, yTop = 12, yBottom = 188;
      // mapeia valores numéricos -> {x, y} dentro do viewport do SVG
      const map = (vals: number[]) => {
        // `max` é a escala usada para converter valor -> posição vertical
        // (troque por um número fixo se quiser um eixo vertical constante)
        const max = Math.max(...vals, 1);
        return vals.map((v, i) => {
          const x = xStart + (i * (xEnd - xStart) / (vals.length - 1));
          const y = yBottom - (v / max) * (yBottom - yTop);
          return { x, y };
        });
      };

      const ptsLight = map(light);
      const ptsDark = map(dark);

      // Converte uma série de pontos em uma string de path SVG suave.
      // Usamos a conversão Catmull–Rom → Bézier para produzir curvas suaves.
      const toPath = (pts: { x: number, y: number }[]) => {
        if (!pts.length) return '';
        if (pts.length === 1) return `M ${pts[0].x} ${pts[0].y}`;

        const dParts: string[] = [];
        dParts.push(`M ${pts[0].x} ${pts[0].y}`);

        // Helper Catmull–Rom -> Bézier cúbico
        const cr2bezier = (p0: any, p1: any, p2: any, p3: any) => {
          const cp1x = p1.x + (p2.x - p0.x) / 6;
          const cp1y = p1.y + (p2.y - p0.y) / 6;
          const cp2x = p2.x - (p3.x - p1.x) / 6;
          const cp2y = p2.y - (p3.y - p1.y) / 6;
          return { cp1x, cp1y, cp2x, cp2y };
        };

        for (let i = 0; i < pts.length - 1; i++) {
          const p0 = pts[i - 1] || pts[i];
          const p1 = pts[i];
          const p2 = pts[i + 1];
          const p3 = pts[i + 2] || p2;
          const c = cr2bezier(p0, p1, p2, p3);
          dParts.push(`C ${c.cp1x} ${c.cp1y} ${c.cp2x} ${c.cp2y} ${p2.x} ${p2.y}`);
        }

        return dParts.join(' ');
      };

      // aplica as strings geradas aos elementos <path> do SVG
      const pathLight = svg.querySelector('.line-light') as SVGPathElement;
      const pathDark = svg.querySelector('.line-dark') as SVGPathElement;
      if (pathLight) pathLight.setAttribute('d', toPath(ptsLight));
      if (pathDark) pathDark.setAttribute('d', toPath(ptsDark));
    } catch (err) {
      console.error('updateSalesSvg error', err);
    }
  }

  // arrays públicas ligadas ao template (12 meses cada)
  // Edite estas arrays para alterar os valores do gráfico. O componente irá
  // regenerar os paths automaticamente no próximo render ou resize.
  lightValues: number[] = [8000,12000,32000,28000,26000,30000,22000,26000,42000,48000,45000,52000];
  darkValues: number[] = [2000,3500,4800,3200,4500,5000,12000,7000,15000,32000,26000,18000];

}
