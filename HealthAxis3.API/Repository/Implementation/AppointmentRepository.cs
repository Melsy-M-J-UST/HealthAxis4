using HealthAxis3.API.Data;
using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository.Implementation
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
