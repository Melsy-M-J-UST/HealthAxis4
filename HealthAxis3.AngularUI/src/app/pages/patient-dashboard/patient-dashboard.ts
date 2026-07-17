import { Component } from '@angular/core';
import { AppointmentService } from '../../services/appointment';
import { DoctorService } from '../../services/doctor';
import { HealthRecordService } from '../../services/health-record';

@Component({
  selector: 'app-patient-dashboard',
  standalone: false,
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboardComponent
{
  constructor(private appointmentService: AppointmentService, private doctorService: DoctorService, private healthRecordService: HealthRecordService) { }

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
  slots: string[] = [];

  newAppointment = {
    specialisation: '',
    doctor: null as any,
    date: '',
    slot: '',
    status: 'Pending',
    patientId: Number(localStorage.getItem('patientId')),
  };
  showCancelModal = false;
  selectedAppointment: any = null;
  cancellationReason = '';

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
  loadAvailableSlots()
  {
    const doctor = this.newAppointment.doctor;
    const date = this.newAppointment.date;
    if (!doctor || !date)
    {
      this.slots = [];
      return;
    }
    this.doctorService.getAvailableSlots(doctor.doctorId, date).subscribe({
      next: (data) =>
      {
        this.slots = data;
      },
      error: (err) =>
      {
        console.error(err);
        this.slots = [];
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

    const appointmentDto = {
      patientId: this.newAppointment.patientId,
      doctorId: this.newAppointment.doctor.doctorId,
      scheduledDate: this.newAppointment.date,
      slot: this.newAppointment.slot,
      status: "Pending"
    };

    this.appointmentService.addAppointment(appointmentDto).subscribe({
      next: () => {
        alert('✅ Appointment booked');
        this.newAppointment = {
          specialisation: '',
          doctor: null as any,
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
  }

  viewHealthRecord(appointment: any)
  {
     this.healthRecordService.getByAppointmentId( appointment.appointmentId).subscribe({
       next: (record: any) =>
       {
         this.selectedRecord = record;
       },
       error: err =>
       {
         console.error(err);
       }
     });
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
    this.filteredDoctors = this.doctors.filter(d => d.specialisation === this.newAppointment.specialisation);
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
    this.filteredSpecialisations = this.specialisations.filter(s => s.toLowerCase().includes(this.searchText.toLowerCase()));
  }

  selectSpecialisation(s: string) {
    this.selectedSpecialisation = s;
    this.searchText = s;
    this.showDropdown = false;
    this.filteredDoctorsList = this.doctors.filter(d => d.specialisation === s);
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

  openCancelModal(appointment: any)
  {
    this.selectedAppointment = appointment;
    this.cancellationReason = '';
    this.showCancelModal = true;
  }

  closeCancelModal()
  {
    this.showCancelModal = false;
    this.selectedAppointment = null;
    this.cancellationReason = '';
  }

  submitCancellation()
  {

    if (!this.cancellationReason.trim())
    {
      alert('Please provide a cancellation reason');
      return;
    }

    this.appointmentService.updateAppointmentStatus(this.selectedAppointment.appointmentId, 'Cancelled', this.cancellationReason)
      .subscribe({
        next: () =>
        {
          alert('Appointment cancelled successfully');
          this.closeCancelModal();
          this.loadData();
        },
        error: (err) =>
        {
          console.error(err);
          alert('Failed to cancel appointment');
        }
      });
  }

}
