import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });
  }

  onRegister() {
    if (this.registerForm.valid) {
      console.log(this.registerForm.value);

      if (this.registerForm.value.password !== this.registerForm.value.confirmPassword) {
        alert("Passwords do not match");
        return;
      }

      // 👉 Replace with API call
      // this.authService.register(this.registerForm.value).subscribe(...)
    }
  }
}
