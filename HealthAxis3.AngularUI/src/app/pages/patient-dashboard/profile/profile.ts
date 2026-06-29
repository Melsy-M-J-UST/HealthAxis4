import { Component } from '@angular/core';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})

export class ProfileComponent {

  userKey = 'patientUser';

  user: any = {};
  ngOnInit() {
    const storedUser = localStorage.getItem(this.userKey);

    if (storedUser) {
      this.user = JSON.parse(storedUser);
    } else {
      // default user
      this.user = {
        name: 'Melsy Maneesha',
        email: 'melsy@email.com',
        dob: '2000-05-10',
        phone: '9876543210',
        insuranceId: 'INS123456',
        gender: 'Female'
      };
    }
  }
  

  // ✅ Calculate age automatically
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
    localStorage.setItem(this.userKey, JSON.stringify(this.user));

    alert('Profile updated successfully ✅');
  }
}
