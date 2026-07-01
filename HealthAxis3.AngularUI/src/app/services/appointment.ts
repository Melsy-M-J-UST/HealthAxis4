import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config/api-config';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  constructor(private http: HttpClient) { }

  private appointments: any[] = [];

  // ✅ LOAD APPOINTMENTS FROM API
  loadAppointments(): Observable<any[]> {
    return this.http.get<any[]>(`${API_BASE_URL}/appointments`)
      .pipe(
        tap((data) => {
          this.appointments = data; // ✅ cache locally
        })
      );
  }

  // ✅ GET ALL (FROM CACHE)
  getAllAppointments() {
    return this.appointments;
  }

  // ✅ TODAY APPOINTMENTS
  getTodayAppointments() {
    const today = new Date().toISOString().split('T')[0];

    return this.appointments.filter(a =>
      a.date === today && a.status !== 'Cancelled'
    );
  }

  // ✅ WEEKLY APPOINTMENTS
  getWeeklyAppointments() {
    const today = new Date();

    return this.appointments.filter(a => {
      const appointmentDate = new Date(a.date);

      const diff =
        (appointmentDate.getTime() - today.getTime()) /
        (1000 * 60 * 60 * 24);

      return diff >= 0 && diff <= 7 && a.status !== 'Cancelled';
    });
  }

  // ✅ CHECK SLOT AVAILABILITY
  checkAvailability(doctor: string, date: string, slot: string): Observable<any> {
    return this.http.get<any>(
      `${API_BASE_URL}/appointments/check-availability`,
      {
        params: { doctor, date, slot }
      }
    );
  }

  // ✅ ADD APPOINTMENT
  addAppointment(data: any): Observable<any> {
    return this.http.post(`${API_BASE_URL}/appointments`, data);
  }

  // ✅ UPDATE APPOINTMENT (CONFIRM / CANCEL / COMPLETE)
  updateAppointment(id: string, data: any): Observable<any> {
    return this.http.put(`${API_BASE_URL}/appointments/${id}`, data);
  }

  // ✅ MY DOCTORS
  getMyDoctors(doctors: any[]) {
    const myDoctorNames = this.appointments.map(a => a.doctor);
    return doctors.filter(d => myDoctorNames.includes(d.name));
  }

}
