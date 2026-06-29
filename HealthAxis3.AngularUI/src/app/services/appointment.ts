import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private appointments: any[] = [];

  constructor() { }

  getAppointments() {
    return this.appointments;
  }

  // ✅ ADD APPOINTMENT
  addAppointment(appointment: any) {

    const conflict = this.appointments.find(a =>
      a.date === appointment.date &&
      a.slot === appointment.slot &&
      a.doctor === appointment.doctor
    );

    if (conflict) {
      return { success: false, message: 'Slot already booked ❌' };
    }

    this.appointments.push({
      ...appointment,
      status: 'Pending',
      healthRecord: {
        diagnosis: '',
        prescription: '',
        notes: ''
      }
    });

    return { success: true, message: 'Appointment booked ✅' };
  }

  // ✅ CANCEL APPOINTMENT
  cancelAppointment(appointment: any) {
    appointment.status = 'Cancelled';
  }

  // ✅ TODAY APPOINTMENTS
  getTodayAppointments() {
    const today = new Date().toISOString().split('T')[0];

    return this.appointments.filter(a => a.date === today);
  }

  // ✅ MY DOCTORS (derived)
  getMyDoctors(doctorsList: any[]) {

    const uniqueDoctors: any[] = [];

    this.appointments.forEach(a => {
      const exists = uniqueDoctors.find(d => d.name === a.doctor);

      if (!exists) {
        const doc = doctorsList.find(d => d.name === a.doctor);
        if (doc) {
          uniqueDoctors.push(doc);
        }
      }
    });

    return uniqueDoctors;
  }

}
