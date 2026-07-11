import { Component } from '@angular/core';
import { AppointmentService } from '../../services/appointment';
import { DoctorService } from '../../services/doctor';

@Component({
  selector: 'app-patient-dashboard',
  standalone: false,
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboardComponent {

  constructor(
    private appointmentService: AppointmentService,
    private doctorService: DoctorService
  ) { }

  appointments: any[] = [];
  todayAppointments: any[] = [];
  weekAppointments: any[] = [];

  doctors: any[] = [];
  specialisations: string[] = [];

  showBookingModal = false;
  showDoctorModal = false;
  showDropdown = false;
  showMyDoctorsModal = false;
  selectedDoctor: any;
  selectedRecord: any = null;

  searchText = '';
  selectedSpecialisation = '';

  filteredDoctors: any[] = [];
  filteredSpecialisations: string[] = [];
  filteredDoctorsList: any[] = [];

  today = new Date().toISOString().split('T')[0];
  slots = ['09:00 AM','10:00 AM', '11:00 AM','12:00 PM', '2:00 PM', '3:00 PM','4:00 PM','5:00 PM'];

  newAppointment = {
    specialisation: '',
    doctorId: 0,
    doctor: '',
    date: '',
    slot: '',
    status: 'Pending',
    patientId: Number(localStorage.getItem('patientId')),
  };

  ngOnInit() {
    this.loadData();
  }

  loadData()
  {
    const patientId = Number(localStorage.getItem('patientId'));
    if (!patientId) return;

    this.doctorService.loadDoctors().subscribe({
      next: () =>
      {
        this.doctors = this.doctorService.getDoctors();
        this.specialisations = this.doctorService.getSpecialisations();
      },
      error: err =>
      {
        console.error('Doctor API error', err);
      }
    });
    this.appointmentService.loadPatientAppointments(patientId).subscribe({
      next: () => {

        this.appointments = this.appointmentService.getAllAppointments();
        this.todayAppointments = this.appointmentService.getTodayAppointments();
        this.weekAppointments = this.appointmentService.getWeeklyAppointments();
      },
      error: (err : Error) => {
        console.error('Error loading appointments', err);
        this.appointments = [];
        this.todayAppointments = [];
        this.weekAppointments = [];
      }
    });
  }
  cancelAppointment(app: any) {
    const updated = { ...app, status: 'Cancelled' };
    this.appointmentService.updateAppointment(app.id, updated).subscribe(() => {
        this.loadData();
      });
  }

  bookAppointment() {
    const { doctor, date, slot } = this.newAppointment;
    if (!doctor || !date || !slot) {
      alert('Please fill all fields ❗');
      return;
    }

    this.appointmentService
      .checkAvailability(doctor, date, slot)
      .subscribe({

        next: (res : any) => {

          if (!res.available) {
            alert('❌ Slot already booked');
            return;
          }

          this.appointmentService
            .addAppointment(this.newAppointment)
            .subscribe({

              next: () => {

                alert('✅ Appointment booked');

                this.newAppointment = {
                  specialisation: '',
                  doctorId: 0,
                  doctor: '',
                  date: '',
                  slot: '',
                  status: 'Pending',
                  patientId: Number(localStorage.getItem('patientId')),
                };

                this.closeBookingModal();
                this.loadData();
              },

              error: () => {
                alert('❌ Booking failed');
              }

            });

        },

        error: () => {
          alert('❌ Availability check failed');
        }

      });
  }

  viewHealthRecord(appointment: any) {
    this.selectedRecord = appointment.healthRecord;
  }

  openBookingModal() {
    this.showBookingModal = true;
  }

  closeBookingModal() {
    this.showBookingModal = false;
  }

  openMyDoctorsModal() {
    this.showMyDoctorsModal = true;
  }

  closeMyDoctorsModal() {
    this.showMyDoctorsModal = false;
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

  onSpecialisationChange() {
    this.filteredDoctors = this.doctors.filter(
      d => d.specialisation === this.newAppointment.specialisation
    );
  }

  onFocus() {
    this.showDropdown = true;
    this.filterSpecialisations();
  }

  onBlur() {
    setTimeout(() => {
      this.showDropdown = false;
    }, 200);
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
      d => d.specialisation === s
    );
  }

  selectAllDoctors() {
    this.selectedSpecialisation = '';
    this.searchText = 'All Doctors';
    this.showDropdown = false;

    this.filteredDoctorsList = this.doctors;
  }

  get myDoctors() {
    return this.appointmentService.getMyDoctors(this.doctors);
  }

}
