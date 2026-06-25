using HealthAxis3.API.Data;
using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class DoctorRepository(AppDbContext context) : Repository<Doctor>(context), IDoctorRepository
    {
        //getavailable doctors
        public async Task<Doctor?> DeactivateAsync(int id, CancellationToken ct = default)
        {
            var doctor = await GetByIdAsync(id, ct);
            if (doctor != null)
            {
                doctor.IsActive = false;
                await context.SaveChangesAsync(ct);
            }
            return doctor;
        }
        public async Task<List<Doctor>> GetByNameAsync(string name, CancellationToken ct = default)
        {
            var existing = await context.Set<Doctor>().Where(e => e.DoctorName == name).ToListAsync(ct);
            return existing;
        }

        public async Task<List<Doctor>> GetBySpecialisationAsync(string specialisation, CancellationToken ct = default)
        {
            var existing = await context.Set<Doctor>().Where(e => e.Specialisation == specialisation).ToListAsync(ct);
            return existing;
        }
        public async Task<List<string>> GetDoctorAvailability(int doctorId, DateTime date, CancellationToken ct)
        {
            var bookedSlots = await context.Appointments
                .Where(a => a.DoctorId == doctorId && a.ScheduledDate.Date == date.Date)
                .Select(a => a.Slot)
                .ToListAsync(ct);

            var availableSlots = Appointment.AllSlots
                .Where(slot => !bookedSlots.Contains(slot))
                .ToList();

            return availableSlots;
        }
    }
}
