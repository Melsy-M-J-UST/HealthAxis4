import { Component } from '@angular/core';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: false,
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})

export class DoctorDashboardComponent {

  appointments: any[] = [
    {
      patient: 'John Doe',
      date: '2026-06-29',
      slot: '11:00 AM',
      status: 'Completed',
      cancelReason: '',
      healthRecord: {
        diagnosis: 'Viral Fever',
        prescription: 'Paracetamol',
        notes: ''
      }
    },
    {
      patient: 'Mary Jane',
      date: '2026-06-30',
      slot: '10:00 AM',
      status: 'Confirmed',
      cancelReason: '',
      healthRecord: {
        diagnosis: '',
        prescription: '',
        notes: ''
      }
    },
    {
      patient: 'John Doe',
      date: '2026-06-28',
      slot: '02:00 PM',
      status: 'Completed',
      cancelReason: '',
      healthRecord: {
        diagnosis: 'Cold',
        prescription: 'Cough Syrup',
        notes: 'Follow-up after 3 days'
      }
    }
  ];

  todayAppointments: any[] = [];
  weekAppointments: any[] = [];

  patients: any[] = [];

  showPatientsModal = false;
  selectedPatientAppointments: any = null;
  selectedAllRecords: any = null;
  selectedRecord: any = null;
  showAppointmentsModal = false;
  // ✅ INIT
  ngOnInit() {

    const today = new Date().toISOString().split('T')[0];

    // ✅ Today's Appointments
    this.todayAppointments = this.appointments.filter(
      a => a.date === today
    );

    // ✅ This week's (simplified for now)
    this.weekAppointments = this.appointments;

    // ✅ Derive patients list
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

  // ✅ MODAL CONTROLS

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
