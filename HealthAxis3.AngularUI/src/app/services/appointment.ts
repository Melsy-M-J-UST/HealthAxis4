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
  selectedDoctor: any;

  loadPatientAppointments(patientId: number): Observable<any[]>
  {
    return this.http.get<any[]>(`${API_BASE_URL}/appointment/patient/${patientId}`).pipe(tap((data) =>
        {
          this.appointments = data;
        })
      );
  }
  loadDoctorAppointments(doctorId: number): Observable<any[]>
  {
    return this.http.get<any[]>(`${API_BASE_URL}/appointment/doctor/${doctorId}`).pipe(tap((data) =>
    {
      this.appointments = data;
    })
    );
  }
  getAllAppointments() {
    return this.appointments;
  }

  getTodayAppointments() {
    const today = new Date().toISOString().split('T')[0];

    return this.appointments.filter(a =>
      a.date === today && a.status !== 'Cancelled'
    );
  }

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

  checkAvailability(doctor: any, date: string, slot: string): Observable<any>
  {
    console.log(doctor);
    console.log(JSON.stringify(doctor));
    console.log('selectedDoctor:', JSON.stringify( doctor));
    const doctorId = doctor.doctorId;
    return this.http.get<any>(
      `${API_BASE_URL}/Doctor/doctors/${doctorId}/availability`,
      {
        params: { doctor, date, slot }
      }
    );
  }

  addAppointment(data: any): Observable<any> {
    return this.http.post(`${API_BASE_URL}/appointment`, data);
  }

  updateAppointment(id: string, data: any): Observable<any> {
    return this.http.put(`${API_BASE_URL}/appointment/${id}`, data);
  }

  getMyDoctors(doctors: any[]) {
    const myDoctorNames = this.appointments.map(a => a.doctor);
    return doctors.filter(d => myDoctorNames.includes(d.name));
  }
}
