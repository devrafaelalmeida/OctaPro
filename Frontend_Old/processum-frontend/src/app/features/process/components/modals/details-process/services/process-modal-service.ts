import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProcessModalService {
  private _process = new BehaviorSubject<any>(null);
  private _visible = new BehaviorSubject<boolean>(false);

  process$ = this._process.asObservable();
  visible$ = this._visible.asObservable();

  open(process: any) {
    this._process.next(process);
    this._visible.next(true);
  }

  close() {
    this._visible.next(false);
  }
}
