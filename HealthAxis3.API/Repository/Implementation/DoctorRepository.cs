using HealthAxis3.API.Data;
using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository.Implementation
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }
    }
}
