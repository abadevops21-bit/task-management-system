import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginResponse } from '../../models/auth.model';
import { UserRegistration, UserLogin } from '../../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/auth'; // Adjust based on your backend

  constructor(private http: HttpClient) {}

  login(credentials: UserLogin): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials);
  }

  register(user: UserRegistration): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  private getStorage(): Storage | null {
    return typeof window !== 'undefined' && window.localStorage ? window.localStorage : null;
  }

  logout(): void {
    const storage = this.getStorage();
    storage?.removeItem('token');
    storage?.removeItem('role');
  }

  isLoggedIn(): boolean {
    const storage = this.getStorage();
    return !!storage?.getItem('token');
  }

  getToken(): string | null {
    const storage = this.getStorage();
    return storage?.getItem('token') ?? null;
  }

  setSession(res: LoginResponse): void {
    const storage = this.getStorage();
    storage?.setItem('token', res.token);
    storage?.setItem('role', res.role);
  }
}