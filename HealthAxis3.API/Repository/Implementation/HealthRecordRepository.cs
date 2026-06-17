using HealthAxis3.API.Data;
using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository.Implementation
{
    public class HealthRecordRepository(AppDbContext context) : Repository<HealthRecord>(context), IHealthRecordRepository
    {
        //GetByIdAsync id
    }
}
