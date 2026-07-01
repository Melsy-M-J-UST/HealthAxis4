import { Component } from '@angular/core';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-sidebar',
  standalone: false,
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})

export class SidebarComponent {
  constructor(private authservice: AuthService) { }
  isLoggedIn() {
    return this.authservice.isLoggedIn();
  }
  isAdmin() {
    return this.authservice.getUserRoles().includes('Admin');
  }
  isPatient() {
    return this.authservice.getUserRoles().includes('Patient');
  }
  isDoctor() {
    return this.authservice.getUserRoles().includes('Doctor');
  }
  scrollTo(id: string) {
    const el = document.getElementById(id);
    if (el) el.scrollIntoView({ behavior: 'smooth' });
  }

}

