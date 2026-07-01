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
    return this.http.get<any[]>(`${API_BASE_URL}/doctors`)
      .pipe(
        tap((data) => {
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
}
