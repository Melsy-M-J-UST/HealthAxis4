import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { UtilService } from '../../services/util';

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
  constructor(private utilService: UtilService, private router: Router) { }

  ngOnInit() {
    const storedUser = localStorage.getItem('patientUser');

    if (storedUser) {
      const data = JSON.parse(storedUser);
      this.userName = data.name;
      this.initials = this.utilService.getInitials(data.name);
    }
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
    this.router.navigate(['/profile']);
    this.showMenu = false;
  }

  logout() {
    if (confirm('Are you sure you want to sign out?')) {
      localStorage.removeItem('patientUser');
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
}
