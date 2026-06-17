using HealthAxis3.API.Data;
using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository.Implementation
{
    public class AppointmentRepository(AppDbContext context) : Repository<Appointment>(context), IAppointmentRepository
    {

        // get by id pat, doc
        //delete
        //update
    }
}
