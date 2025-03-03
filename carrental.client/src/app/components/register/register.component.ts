import { Component } from '@angular/core';
import { RegisterRequestDto } from '../../models/register-request-dto';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onRegister() {
    if (this.registerForm.invalid) return;

    const dto: RegisterRequestDto = this.registerForm.value;

    this.authService.register(dto).subscribe({
      next: () => {
        this.successMessage = 'Zarejestrowano pomyślnie!';
        this.errorMessage = '';
        this.registerForm.reset();
      },
      error: (err) => {
        this.errorMessage = err.error || err.message;
        this.successMessage = '';
      }
    });
  }
}
