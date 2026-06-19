using HealthAxis3.API.Data;
using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class HealthRecordRepository(AppDbContext context) : Repository<HealthRecord>(context), IHealthRecordRepository
    {
        public async Task<List<HealthRecord>> GetByDoctorIdAsync(int id, CancellationToken ct = default)
        {
            var existing = await context.Set<HealthRecord>().Where(e => e.DoctorId == id).ToListAsync(ct);
            return existing;
        }
        public async Task<List<HealthRecord>> GetByPatientIdAsync(int id, CancellationToken ct = default)
        {
            var existing = await context.Set<HealthRecord>().Where(e => e.PatientId == id).ToListAsync(ct);
            return existing;
        }
    }
}
