import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MaintblService {

  constructor() { }

  getAbc(): string {
    return 'Goodnight';
  }
}
