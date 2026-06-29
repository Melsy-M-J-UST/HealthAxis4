import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilService {

  constructor() { }

  getInitials(name: string): string {
    if (!name) return '';

    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase();
  }
}
