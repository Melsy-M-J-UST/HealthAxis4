export interface AuthResponse {
  accessToken: string;
  message: string;
  expiresIn: number;
  role: string;
  userId: string;
  patientId: number;
  doctorId: number;

}
