using HealthAxis3.API.Data;
using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository.Implementation
{
    public class PatientRepository(AppDbContext context) : Repository<Patient>(context), IPatientRepository
    {
    }
}
