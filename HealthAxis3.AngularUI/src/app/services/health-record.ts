import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api-config';
import { AppointmentService } from './appointment';
import { DoctorService } from './doctor';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService
{
  constructor(private http: HttpClient, private appointmentService: AppointmentService, private doctorService: DoctorService) { }
  addHealthRecord(data: any): Observable<any>
  {
    return this.http.post(`${API_BASE_URL}/HealthRecord`, data);
  }
  getByAppointmentId(appointmentId: number)
  {
    return this.http.get(`${API_BASE_URL}/HealthRecord/appointment/${appointmentId}`);
  }
}
