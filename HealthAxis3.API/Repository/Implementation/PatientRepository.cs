using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis3.API.Repository.Implementation
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(DbContext context) : base(context)
        {
        }
    }
}
