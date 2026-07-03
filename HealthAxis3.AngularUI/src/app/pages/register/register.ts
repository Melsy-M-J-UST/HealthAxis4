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
      patientName: ['', [
        Validators.required,
        Validators.pattern('^[A-Za-z ]{3,}$')
      ]],

      dateOfBirth: ['', [
        Validators.required,
        this.futureDateValidator
      ]],


      gender: ['', Validators.required],

      email: ['', [
        Validators.required,
        Validators.email
      ]],

      phoneNumber: ['', [
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

  futureDateValidator(control: any) {
    if (!control.value) return null;

    const selectedDate = new Date(control.value);
    const today = new Date();

    today.setHours(0, 0, 0, 0);

    return selectedDate <= today
      ? null
      : { futureDate: true };
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
      patientName: this.registerForm.value.patientName,
      dateOfBirth: this.registerForm.value.dateOfBirth,
      gender: this.registerForm.value.gender,
      email: this.registerForm.value.email,
      phoneNumber: this.registerForm.value.phoneNumber,
      insuranceId: this.registerForm.value.insuranceId,
      password: this.registerForm.value.password,
      confirmPassword: this.registerForm.value.confirmPassword,
      role: "Patient"
    };

    this.authService.register(payload).subscribe({
      next: () => {
        alert('✅ Registration successful');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        alert('❌ Registration failed');

        console.log('Status:', err.status);
        console.log('Full Error:', err);
        console.log('Response Body:', err.error);

      }
    });
  }

}
