import { Component } from '@angular/core';
import { UtilService } from './src/app/services/util';


@Component({
  selector: 'app-patient-dashboard',
  standalone: false,
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboardComponent {

  userKey = 'patientUser';

  user: any = {
    name: '',
    initials: ''
  };
  constructor(private utilService: UtilService) { }

  ngOnInit() {
    const storedUser = localStorage.getItem(this.userKey);

    if (storedUser) {
      const data = JSON.parse(storedUser);

      this.user.name = data.name;
      this.user.initials = this.utilService.getInitials(data.name);
    }
  }

  showMenu = false;
  showPasswordModal = false;
  showLogoutModal = false;

  get todayAppointments() {
    const today = new Date().toISOString().split('T')[0];

    return this.appointments.filter(a => a.date === today);
  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
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

  cancelAppointment(app: any) {
    alert('Appointment cancelled');
  }

  showBookingModal = false;

  specialisations = ['Cardiology', 'Dermatology', 'General'];

  doctors = [
    { name: 'Dr. John', specialization: 'Cardiology' },
    { name: 'Dr. Anu', specialization: 'Dermatology' },
    { name: 'Dr. Ravi', specialization: 'General' }
  ];

  filteredDoctors: any[] = [];
  today = new Date().toISOString().split('T')[0];
  slots = ['10:00 AM', '11:00 AM', '2:00 PM', '3:00 PM'];

  appointments: any[] = [];

  newAppointment = {
    specialization: '',
    doctor: '',
    date: '',
    slot: ''
  };

  /* OPEN MODAL */
  openBookingModal() {
    this.showBookingModal = true;
  }

  /* CLOSE MODAL */
  closeBookingModal() {
    this.showBookingModal = false;
  }

  /* FILTER DOCTORS */
  onSpecializationChange() {
    this.filteredDoctors = this.doctors.filter(
      d => d.specialization === this.newAppointment.specialization
    );
  }

  /* BOOK APPOINTMENT */
  bookAppointment() {

    // ✅ Check slot availability
    const conflict = this.appointments.find(a =>
      a.date === this.newAppointment.date &&
      a.slot === this.newAppointment.slot &&
      (a.doctor === this.newAppointment.doctor)
    );

    if (conflict) {
      alert('Slot already booked for this doctor ❌');
      return;
    }

    // ✅ Add appointment
    this.appointments.push({
      ...this.newAppointment,
      date: this.newAppointment.date,
      status: 'Pending',
      healthRecord: {
        diagnosis: '',
        prescription: '',
        notes: ''
      }
    });
    alert('Appointment Booked ✅');

    // Reset
    this.newAppointment = {
      specialization: '',
      doctor: '',
      date: '',
      slot: ''
    };

    this.closeBookingModal();
  }
  selectedRecord: any = null;

  viewHealthRecord(appointment: any) {
    this.selectedRecord = appointment.healthRecord;
  }
  showDoctorModal = false;
  showDropdown = false;
  searchText = '';
  selectedSpecialisation = '';

  filteredSpecialisations: string[] = [];
  filteredDoctorsList: any[] = [];

  onFocus() {
    this.showDropdown = true;
    this.filterSpecialisations();
  }

  onBlur() {
    setTimeout(() => {
      this.showDropdown = false;
    }, 200);
  }

  openDoctorSearch() {
    this.showDoctorModal = true;

    this.searchText = 'All Doctors';
    this.filteredDoctorsList = this.doctors;
    this.filteredSpecialisations = this.specialisations;
  }

  closeDoctorSearch() {
    this.showDoctorModal = false;
  }

  filterSpecialisations() {
    this.filteredSpecialisations = this.specialisations.filter(s =>
      s.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }


  selectSpecialisation(s: string) {
    this.selectedSpecialisation = s;
    this.searchText = s;
    this.showDropdown = false;

    this.filteredDoctorsList = this.doctors.filter(
      d => d.specialization === s
    );
  }

  selectAllDoctors() {
    this.selectedSpecialisation = '';
    this.searchText = 'All Doctors';
    this.showDropdown = false;

    // ✅ show all doctors
    this.filteredDoctorsList = this.doctors;
  }
  get myDoctors() {
    const uniqueDoctors: any[] = [];

    this.appointments.forEach(a => {
      const exists = uniqueDoctors.find(d => d.name === a.doctor);

      if (!exists) {
        // find doctor details from master list
        const doctorDetails = this.doctors.find(
          d => d.name === a.doctor
        );

        if (doctorDetails) {
          uniqueDoctors.push(doctorDetails);
        }
      }
    });

    return uniqueDoctors;
  }
  showMyDoctorsModal = false;

  openMyDoctorsModal() {
    this.showMyDoctorsModal = true;
  }

  closeMyDoctorsModal() {
    this.showMyDoctorsModal = false;
  }
}
