using HealthAxis3.API.Data;
using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class PatientRepository(AppDbContext context) : Repository<Patient>(context), IPatientRepository
    {
        public async Task<Patient?> DeactivateAsync(int id, CancellationToken ct = default)
        {
            var patient = await GetByIdAsync(id, ct);
            if (patient != null)
            {
                patient.IsActive = !patient.IsActive;
                await context.SaveChangesAsync(ct);
            }
            return patient;
        }

        public async Task<List<Patient>> GetByNameAsync(string name, CancellationToken ct = default)
        {
            var existing = await context.Set<Patient>().Where(e => e.PatientName == name).ToListAsync(ct);
            return existing;
        }
        public async Task<List<Patient>> GetByPhoneAsync(string phone, CancellationToken ct = default)
        {
            var existing = await context.Set<Patient>().Where(e => e.PhoneNumber == phone).ToListAsync(ct);
            return existing;
        }
    }
}
