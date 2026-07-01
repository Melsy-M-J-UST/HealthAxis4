import { Component } from '@angular/core';
import { PatientService } from '../../../services/patient';
import { AuthService } from '../../../services/auth';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})


export class ProfileComponent {

  constructor(
    private patientService: PatientService,
    private authService: AuthService
  ) {}

  user: any = {};
  saved = false;
  userId: string | null = null;

  ngOnInit() {

    this.userId = this.getUserIdFromToken();
    this.loadProfile();
  }

  getUserIdFromToken(): string | null {

    const token = this.authService.getToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));

    return payload.sub || payload.userId;
  }

  loadProfile() {

    if (!this.userId) return;

    this.patientService.getPatientById(this.userId)
      .subscribe({
        next: (data) => {
          this.user = data;
        },
        error: (err) => {
          console.error('Error loading profile', err);
        }
      });
  }

  get age(): number {

    if (!this.user.dob) return 0;

    const birthDate = new Date(this.user.dob);
    const today = new Date();

    let age = today.getFullYear() - birthDate.getFullYear();
    const month = today.getMonth() - birthDate.getMonth();

    if (month < 0 || (month === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age;
  }


  saveProfile() {

    if (!this.userId) return;

    this.patientService.updatePatient(this.userId, this.user)
      .subscribe({
        next: () => {
          this.saved = true;
          alert('✅ Profile updated');
        },
        error: () => {
          alert('❌ Failed to update profile');
        }
      });
  }


}
