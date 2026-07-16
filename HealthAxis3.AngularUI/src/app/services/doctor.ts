import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config/api-config';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  constructor(private http: HttpClient) { }

  private doctors: any[] = [];

  loadDoctors(): Observable<any[]> {
    return this.http.get<any[]>(`${API_BASE_URL}/Doctor`).pipe( tap((data : any) => {
          this.doctors = data;
        })
      );
  }

  getDoctors() {
    return this.doctors;
  }

  getSpecialisations(): string[] {
    const specs = this.doctors.map(d => d.specialisation);
    return [...new Set(specs)];
  }

  getAvailableSlots( doctorId: number, date: string): Observable<string[]>
  {
    return this.http.get<string[]>(`${API_BASE_URL}/Doctor/doctors/${doctorId}/availability`,
      {
        params: {
          date: date
        }
      }
    );
  }
  getDoctorById(doctorId: number): Observable<any>
  {
    return this.http.get<any>(`${API_BASE_URL}/Doctor/${doctorId}`);
  }
}
