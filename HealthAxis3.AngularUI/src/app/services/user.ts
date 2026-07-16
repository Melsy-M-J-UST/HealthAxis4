import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {

   private getStorageKey(): string
  {
    const role = localStorage.getItem('role');
    if (role === 'Doctor')
    {
      return 'doctorUser';
    }
    return 'patientUser';
  }
  private user: any = null;
  constructor() {
    this.loadUser();
  }

  private loadUser(): void
  {
    const storedUser = localStorage.getItem(this.getStorageKey());
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
    return name.split(' ').map((n: string) => n[0]).join('').toUpperCase();
  }

  setUser(userData: any): void {
    this.user = userData;
    localStorage.setItem(this.getStorageKey(), JSON.stringify(userData));
  }

  updateUserField(field: string, value: any): void {
    if (!this.user) return;
    this.user[field] = value;
    localStorage.setItem(this.getStorageKey(), JSON.stringify(this.user));
  }

  refreshUser(): void {
    this.loadUser();
  }

  clearUser(): void {
    this.user = null;
    localStorage.removeItem(this.getStorageKey());
  }

  isLoggedIn(): boolean {
    return !!this.user;
  }

}
