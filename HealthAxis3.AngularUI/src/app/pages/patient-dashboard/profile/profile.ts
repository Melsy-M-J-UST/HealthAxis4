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
  saved = false;
    userService: any;
  ngOnInit() {
    const storedUser =this.userService.getUser(this.userKey);

    if (storedUser) {
      this.user = JSON.parse(storedUser);
    } else {
      this.user = {
        name: 'Melsy Maneesha',
        email: 'melsy@email.com',
        dob: '2004-02-19',
        phone: '9876543210',
        insuranceId: 'INS1234',
        gender: 'Female'
      };
    }
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
    this.userService.setUser(this.user);
  }
}
