import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../config/api-config';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  constructor(private http: HttpClient) { }

  private patients: any[] = [];

  loadPatients(): Observable<any[]> {
    return this.http.get<any[]>(`${API_BASE_URL}/patients`)
      .pipe(
        tap((data) => {
          this.patients = data; 
        })
      );
  }

  getAllPatients() {
    return this.patients;
  }

  getPatientById(id: number): Observable<any> {
    return this.http.get(`${API_BASE_URL}/Patient/${id}`);
  }

  updatePatient(id: number, data: any): Observable<any> {
    return this.http.put(`${API_BASE_URL}/Patient/${id}`, data);
  }

}
