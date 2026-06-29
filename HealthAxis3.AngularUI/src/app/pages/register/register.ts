import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      name: ['Name should only contain alphabets and spaces and should be a minimum of 3 letters', Validators.required],
      email: ['Enter a valid email', [Validators.required, Validators.email]],
      password: ['Password should be minimum of 8 and maximum of 12 characters. It should include at least one uppercase, one lowercase, a number and a special character.', Validators.required],
      confirmPassword: ['Passwords do not match', Validators.required]
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
