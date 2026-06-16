using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis3.API.Repository.Implementation
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(DbContext context) : base(context)
        {
        }
    }
}
