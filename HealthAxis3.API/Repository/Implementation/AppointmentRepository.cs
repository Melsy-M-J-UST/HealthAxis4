using HealthAxis3.API.Data;
using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis3.API.Repository.Implementation
{
    public class AppointmentRepository(AppDbContext context) : Repository<Appointment>(context), IAppointmentRepository
    {
        //delete
        //update
        public async Task<List<Appointment>> GetByDoctorIdAsync(int id, CancellationToken ct = default)
        {
            var existing = await context.Set<Appointment>().Where(e => e.DoctorId == id).ToListAsync(ct);
            return existing;
        }
        public async Task<List<Appointment>> GetByPatientIdAsync(int id, CancellationToken ct = default)
        {
            var existing = await context.Set<Appointment>().Where(e => e.PatientId == id).ToListAsync(ct);
            return existing;
        }
    }
}
