import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../dtos/login-request';
import { AuthResponse } from '../dtos/auth-response';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api-config';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) { }
  login(loginRequest: LoginRequest):Observable<AuthResponse>{
    return this.http.post <AuthResponse>(`${API_BASE_URL}/auth/login`, loginRequest);
  }
  setSession(authResponse: AuthResponse) {
    localStorage.setItem("token", authResponse.accessToken);
    if (authResponse.patientId) {
      localStorage.setItem("patientId", authResponse.patientId.toString());
    }
  }
  getToken() : string|null{
    return localStorage.getItem("token");
  }
  isLoggedIn(): boolean {
    return localStorage.getItem("token") != null;
  }
  logout() {
    localStorage.removeItem("token");
  }
  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    const payload = this.parseJwt(token);

    const exp = payload?.exp;

    if (!exp) return true;

    const currentTime = Math.floor(Date.now() / 1000);
    console.log('exp:', exp);
    console.log('now:', Math.floor(Date.now() / 1000));
    
    return exp < currentTime;
  }
  private parseJwt(token: string): any | null {
    try {
      const payload = token.split(".")[1];
      const json = atob(payload).replace(/-/g, '+').replace(/_/g, '/');
      return JSON.parse(json);
    }
    catch {
      return null;
    }
    
  }
  getUserRoles() {
    const token = this.getToken();
    if (!token) return [];
    const payload = this.parseJwt(token);
    if (!payload) return [];

    const roleClaims = payload.role ?? payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    return Array.isArray(roleClaims) ? roleClaims : [roleClaims];
  }
  register(data: any) {
    return this.http.post(`${API_BASE_URL}/auth/register`, data);
  }
}
