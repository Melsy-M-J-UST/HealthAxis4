import { Component } from '@angular/core';
import { AppointmentService } from '../../services/appointment';
import { DoctorService } from '../../services/doctor';
import { HealthRecordService } from '../../services/health-record';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: false,
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})

export class DoctorDashboardComponent {

  constructor(private appointmentService: AppointmentService, private doctorService: DoctorService, private healthRecordService: HealthRecordService) { }

  appointments: any[] = [];
  todayAppointments: any[] = [];
  weekAppointments: any[] = [];
  patients: any[] = [];
  doctor: any;

  showPatientsModal = false;
  selectedPatientAppointments: any = null;
  selectedAllRecords: any = null;
  selectedRecord: any = null;
  showAppointmentsModal = false;
  showCompleteModal = false;
  selectedAppointment: any = null;

  healthRecord = {
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  ngOnInit() {
    this.loadData();
  }

  loadData()
  {
    const doctorId = Number(localStorage.getItem('doctorId'));
    console.log('Doctor ID:', doctorId);
    if (!doctorId) return;
    this.doctorService.getDoctorById(doctorId).subscribe({
      next: (data) =>
      {
        console.log('Doctor Loaded', data);
        this.doctor = data;
      },
      error: (err) =>
      {
        console.error(err);
      }
    });
    this.appointmentService.loadDoctorAppointments(doctorId).subscribe({
      next: () => {
        this.appointments = this.appointmentService.getAllAppointments();
        console.log('Appointments:', this.appointments);
        this.todayAppointments = this.appointmentService.getTodayAppointments();
        this.weekAppointments = this.appointmentService.getWeeklyAppointments();
        this.preparePatients();
      },
      error: (err: Error) => {
        console.error('Error loading appointments', err);
        this.appointments = [];
        this.todayAppointments = [];
        this.weekAppointments = [];
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

  updateStatus(appointment: any, status: string)
  {

    this.appointmentService.updateAppointmentStatus(appointment.appointmentId, status).subscribe({
        next: () =>
        {
          appointment.status = status;
        },
        error: err => console.error(err)
      });
  }
  completeAppointment()
  {
    const payload = {
      appointmentId: this.selectedAppointment.appointmentId,
      patientId: this.selectedAppointment.patientId,
      doctorId: this.selectedAppointment.doctorId,
      visitDate: this.selectedAppointment.scheduledDate,
      diagnosis: this.healthRecord.diagnosis,
      prescription: this.healthRecord.prescription,
      notes: this.healthRecord.notes
    };
    console.log(payload);
    this.healthRecordService.addHealthRecord(payload).subscribe({
      next: () =>
      {
      this.appointmentService.updateAppointmentStatus(this.selectedAppointment.appointmentId, 'Completed').subscribe(
        {
          next: () =>
          {
            this.selectedAppointment.status = 'Completed';
            this.closeCompleteModal();
            this.loadData();
          },
          error: err => console.error(err)
        });
      },
      error: err =>
      {
        console.log('ERROR BODY');
        console.log(err.error);

        console.log(
          JSON.stringify(err.error, null, 2)
        );
      }
    });
  }
  openCompleteModal(appointment: any)
  {
    this.selectedAppointment = appointment;

    this.healthRecord = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };

    this.showCompleteModal = true;
  }

  closeCompleteModal()
  {
    this.showCompleteModal = false;
    this.selectedAppointment = null;
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
