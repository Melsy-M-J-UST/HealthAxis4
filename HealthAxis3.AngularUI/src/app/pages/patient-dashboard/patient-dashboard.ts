import { Component } from '@angular/core';
import { UtilService } from '../../services/util';
import { AppointmentService } from '../../services/appointment';


@Component({
  selector: 'app-patient-dashboard',
  standalone: false,
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboardComponent {

  // userKey = 'patientUser';

  // user: any = {
  //   name: '',
  //   initials: ''
  // };
  constructor(
    private appointmentService: AppointmentService,
    private utilService: UtilService
  ) { }

  ngOnInit() {
    // const storedUser = localStorage.getItem(this.userKey);

    // if (storedUser) {
    //   const data = JSON.parse(storedUser);

    //   this.user.name = data.name;
    //   this.user.initials = this.utilService.getInitials(data.name);
    // }
    // this.loadUser();
  }

  get todayAppointments() {
    return this.appointmentService.getTodayAppointments();
  }

  cancelAppointment(app: any) {
    this.appointmentService.cancelAppointment(app);
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

  get appointments() {
    return this.appointmentService.getAppointments();
  }

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

    const result = this.appointmentService.addAppointment(this.newAppointment);

    if (!result.success) {
      alert(result.message);
      return;
    }

    alert(result.message);

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
    return this.appointmentService.getMyDoctors(this.doctors);
  }
  showMyDoctorsModal = false;

  openMyDoctorsModal() {
    this.showMyDoctorsModal = true;
  }

  closeMyDoctorsModal() {
    this.showMyDoctorsModal = false;
  }
}
