import { Component } from '@angular/core';
import { LoginRequestDto } from '../../models/login-request-dto';
import { AuthService } from '../../services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onLogin() {
    if (this.loginForm.invalid) return;

    const dto: LoginRequestDto = this.loginForm.value;

    this.authService.login(dto).subscribe({
      next: (res) => {
        this.authService.setToken(res.token);
        this.successMessage = 'Zalogowano pomyślnie';
        this.errorMessage = '';
        this.loginForm.reset();
        this.router.navigate(['/vehicles']);
      },
      error: (err) => {
        this.errorMessage = err.error || err.message;
        this.successMessage = '';
      }
    });
  }
}
