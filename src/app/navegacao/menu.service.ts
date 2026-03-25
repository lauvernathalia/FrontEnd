import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MenuService {
  // null = no override (use router-based visibility), boolean = explicit override
  private overrideSubject = new BehaviorSubject<boolean | null>(null);
  override$ = this.overrideSubject.asObservable();

  setOverride(value: boolean | null) {
    this.overrideSubject.next(value);
  }
}
