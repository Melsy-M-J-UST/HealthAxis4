namespace HealthAxis3.API.Events
{
    using HealthAxis3.API.Data;
    using HealthAxis3.API.Models;
    using MassTransit;

    public class AppointmentBookedConsumer
        : IConsumer<AppointmentBookedEvent>
    {
        private readonly AppDbContext _dbContext;

        public AppointmentBookedConsumer(
            AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(
            ConsumeContext<AppointmentBookedEvent> context)
        {
            var message = context.Message;

            var notification = new Notification
            {
                DoctorId = message.DoctorId,
                Message =
                    $"New appointment booked by {message.PatientName} on {message.ScheduledDate:dd-MM-yyyy} at {message.TimeSlot}",
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Notifications.Add(notification);

            await _dbContext.SaveChangesAsync();
        }
    }
}
