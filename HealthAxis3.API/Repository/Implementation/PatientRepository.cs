using HealthAxis3.API.Data;
using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository.Implementation
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
        }
    }
}
