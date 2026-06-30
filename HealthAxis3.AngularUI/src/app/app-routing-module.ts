import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';
import { PatientDashboardComponent } from './pages/patient-dashboard/patient-dashboard';
import { ProfileComponent } from './pages/patient-dashboard/profile/profile';
import { DoctorDashboardComponent } from './pages/doctor-dashboard/doctor-dashboard';

const routes: Routes = [
  { path: '', component: HomeComponent }, // Home default
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: PatientDashboardComponent },
  { path: 'dashboard/profile', component: ProfileComponent },
  { path: 'doctordashboard', component: DoctorDashboardComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
