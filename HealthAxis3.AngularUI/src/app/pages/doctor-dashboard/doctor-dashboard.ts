import { Component } from '@angular/core';
import { AppointmentService } from '../../services/appointment';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: false,
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})

export class DoctorDashboardComponent {

  constructor(private appointmentService: AppointmentService) { }

  appointments: any[] = [];
  todayAppointments: any[] = [];
  weekAppointments: any[] = [];
  patients: any[] = [];

  showPatientsModal = false;
  selectedPatientAppointments: any = null;
  selectedAllRecords: any = null;
  selectedRecord: any = null;
  showAppointmentsModal = false;

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.appointmentService.loadAppointments().subscribe({
      next: () => {

        this.appointments = this.appointmentService.getAllAppointments();
        this.todayAppointments = this.appointmentService.getTodayAppointments();
        this.weekAppointments = this.appointmentService.getWeeklyAppointments();

      },
      error: (err) => {
        console.error('Error loading appointments', err);
      }
    });
  }

  preparePatients() {
    const map = new Map();

    this.appointments.forEach(a => {

      if (!map.has(a.patient)) {
        map.set(a.patient, {
          name: a.patient,
          lastVisit: a.date,
          appointments: []
        });
      }

      map.get(a.patient).appointments.push(a);
    });

    this.patients = Array.from(map.values());
  }

  openPatientsModal() {
    this.showPatientsModal = true;
  }

  closePatientsModal() {
    this.showPatientsModal = false;
  }

  openPatientAppointments(patient: any) {
    this.selectedPatientAppointments = patient.appointments;
  }

  openAllRecords(patient: any) {
    this.selectedAllRecords = patient.appointments;
  }

  viewHealthRecord(appointment: any) {
    this.selectedRecord = appointment.healthRecord;
  }

  openAppointmentsModal() {
    this.showAppointmentsModal = true;
  }

  closeAppointmentsModal() {
    this.showAppointmentsModal = false;
  }

}
