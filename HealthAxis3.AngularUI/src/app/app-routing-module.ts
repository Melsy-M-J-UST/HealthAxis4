import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';
import { PatientDashboardComponent } from './pages/patient-dashboard/patient-dashboard';
import { ProfileComponent } from './pages/patient-dashboard/profile/profile';
import { DoctorDashboardComponent } from './pages/doctor-dashboard/doctor-dashboard';
import { authGuard, roleGuard } from './guards/auth-guard';

const routes: Routes = [
  { path: '', component: HomeComponent }, // Home default
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'dashboard', component: PatientDashboardComponent, canActivate: [authGuard, roleGuard], data: { roles: ['Patient'] } },
  { path: 'dashboard/profile', component: ProfileComponent, canActivate: [authGuard, roleGuard], data: { roles: ['Patient'] } },
  { path: 'doctordashboard', component: DoctorDashboardComponent, canActivate: [authGuard, roleGuard], data: { roles: ['Doctor'] } },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
