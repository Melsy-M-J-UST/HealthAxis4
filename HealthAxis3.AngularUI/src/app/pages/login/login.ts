import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { ActivatedRoute, Router } from '@angular/router';
import { ADMIN_URL } from '../../config/api-config';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  fb: FormBuilder = inject(FormBuilder);
  private authService: AuthService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  errorMessage: string = '';
    ngOnInit(): void {
      this.loginForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required]
      });
    }

  onLogin() {
    this.authService.login(this.loginForm.value).subscribe({
      next: (response: any) => {
        console.log('Login success ✅');
        console.log(response)
        this.authService.setSession(response)
        const returnUrl = this.route.snapshot.queryParams['returnUrl'];
        console.log('Return URL:', returnUrl);
        if (returnUrl) {
          this.router.navigateByUrl(returnUrl);
        } else {
          const roles = this.authService.getUserRoles();
          console.log('Roles:', roles);
          if (roles.includes('Doctor'))
          {
            this.router.navigate(['/doctordashboard']);
          }
          else if (roles.includes('Patient'))
          {
            this.router.navigate(['/dashboard']);
          }
          else if (roles.includes('Admin'))
          {
            this.router.navigate([ADMIN_URL]);
          }
        }
      },
      error: (errorMessage: Error) => {
        this.errorMessage = "Invalid Login Credentials";
        console.log(this.errorMessage);
      }
    });
  }
}
