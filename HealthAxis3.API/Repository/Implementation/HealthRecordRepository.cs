using HealthAxis3.API.Data;
using HealthAxis3.API.Models;

namespace HealthAxis3.API.Repository.Implementation
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(AppDbContext context) : base(context)
        {
        }
    }
}
