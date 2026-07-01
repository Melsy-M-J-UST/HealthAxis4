import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})

export class RegisterComponent {

  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    this.registerForm = this.fb.group({
      name: ['', [
        Validators.required,
        Validators.pattern('^[A-Za-z ]{3,}$')
      ]],

      dob: ['', Validators.required],

      gender: ['', Validators.required],

      email: ['', [
        Validators.required,
        Validators.email
      ]],

      phone: ['', [
        Validators.required,
        Validators.pattern('^[6-9][0-9]{9}$')
      ]],

      insuranceId: [''],

      password: ['', [
        Validators.required,
        Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,12}$')
      ]],

      confirmPassword: ['', Validators.required]

    }, { validators: this.passwordMatchValidator });

  }

  passwordMatchValidator(form: AbstractControl) {
    const password = form.get('password')?.value;
    const confirm = form.get('confirmPassword')?.value;

    return password === confirm ? null : { mismatch: true };
  }

  onRegister() {

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const payload = {
      name: this.registerForm.value.name,
      dob: this.registerForm.value.dob,
      gender: this.registerForm.value.gender,
      email: this.registerForm.value.email,
      phone: this.registerForm.value.phone,
      insuranceId: this.registerForm.value.insuranceId,
      password: this.registerForm.value.password
    };

    this.authService.register(payload).subscribe({
      next: () => {
        alert('✅ Registration successful');
        this.router.navigate(['/login']);
      },
      error: () => {
        alert('❌ Registration failed');
      }
    });
  }

}
