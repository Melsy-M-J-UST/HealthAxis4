import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {

  private storageKey = 'patientUser';

  private user: any = null;

  constructor() {
    this.loadUser();
  }

  private loadUser(): void {
    const storedUser = localStorage.getItem(this.storageKey);

    if (storedUser) {
      this.user = JSON.parse(storedUser);
    } else {
      this.user = null;
    }
  }

  getUser(): any {
    return this.user;
  }

  getUserName(): string {
    return this.user?.name || '';
  }

  getInitials(): string {
    const name = this.getUserName();

    if (!name) return '';

    return name
      .split(' ')
      .map((n: string) => n[0])
      .join('')
      .toUpperCase();
  }

  setUser(userData: any): void {
    this.user = userData;
    localStorage.setItem(this.storageKey, JSON.stringify(userData));
  }

  updateUserField(field: string, value: any): void {
    if (!this.user) return;

    this.user[field] = value;
    localStorage.setItem(this.storageKey, JSON.stringify(this.user));
  }

  refreshUser(): void {
    this.loadUser();
  }

  clearUser(): void {
    this.user = null;
    localStorage.removeItem(this.storageKey);
  }

  isLoggedIn(): boolean {
    return !!this.user;
  }

}
