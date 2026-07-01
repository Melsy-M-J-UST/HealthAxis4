import { Component, HostListener, Input } from '@angular/core';
import { Router } from '@angular/router';
import { UtilService } from '../../services/util';
import { UserService } from '../../services/user';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.html',
  styleUrls: ['./header.css']
})

export class HeaderComponent {

  @Input() showMenuEnabled: boolean = true; 
  initials = '';
  userName = '';
  showMenu = false;
  showPasswordModal = false;
  showLogoutModal = false;
  user: any = {};
  constructor(private router: Router, private userService: UserService, private authservice: AuthService ) { }

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
  // loadUser() {
  //   const storedUser = this.userService.getUserName('patientUser');

  //   if (storedUser) {
  //     const data = JSON.parse(storedUser);

  //     this.userName = data.name;
  //     this.initials = this.userService.getInitials();
  //   }
  // }

  ngOnInit() {
    this.userName = this.userService.getUserName();
    this.initials = this.userService.getInitials();
    this.user = this.userService.getUser();
  }

  openPasswordModal() {
    this.showPasswordModal = true;
  }

  openLogoutModal() {
    this.showLogoutModal = true;
  }

  closeModals() {
    this.showLogoutModal = false;
    this.showPasswordModal = false;
  }
  toggleMenu() {
    if (this.showMenuEnabled) {
      this.showMenu = !this.showMenu;
    }
  }

  @HostListener('document:click', ['$event'])
  handleClick(event: Event) {
    const target = event.target as HTMLElement;

    if (!target.closest('.profile')) {
      this.showMenu = false;
    }
  }

  goToProfile() {
    this.router.navigate(['/dashboard/profile']);
    this.showMenu = false;
  }

  logout() {
    if (confirm('Are you sure you want to log out?')) {
      this.userService.clearUser();
      this.router.navigate(['/login']);
    }
  }

  openPassword() {
    alert('Open Change Password Modal');
  }

  showNotifications = false;

  notifications: string[] = [
    'Appointment booked ✅',
    'Reminder: Today appointment at 10 AM'
  ];

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }
  saveProfile() {
    this.userService.setUser(this.user);

    // ✅ refresh header instantly
    this.userName = this.userService.getUserName();
    this.initials = this.userService.getInitials();

    this.closeModals();
  }
}
