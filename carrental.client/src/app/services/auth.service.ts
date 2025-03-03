import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { RegisterRequestDto } from "../models/register-request-dto";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { LoginRequestDto } from "../models/login-request-dto";
import { AuthResponseDto } from "../models/auth-response-dto";
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = environment.apiUrl;
  private tokenKey = 'jwt_token';
  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  register(dto: RegisterRequestDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, dto);
  }

  login(dto: LoginRequestDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/auth/login`, dto);
  }

  setToken(token: string): void {
    localStorage.setItem('jwt_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUserId(): number | null {
    const token = this.getToken();
    if (!token) {
      return null;
    }
    const decoded = this.jwtHelper.decodeToken(token);
    const userIdClaim = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
      || decoded['sub'];
    if (!userIdClaim) {
      return null;
    }
    return parseInt(userIdClaim, 10);
  }
}
