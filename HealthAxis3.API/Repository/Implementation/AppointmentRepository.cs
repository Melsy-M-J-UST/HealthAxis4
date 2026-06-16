using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis3.API.Repository.Implementation
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbContext context) : base(context)
        {
        }
    }
}
