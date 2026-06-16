using HealthAxis3.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis3.API.Repository.Implementation
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(DbContext context) : base(context)
        {
        }
    }
}
