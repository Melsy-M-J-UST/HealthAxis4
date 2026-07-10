using HealthAxis3.API.Data;
using HealthAxis3.API.Events;
using HealthAxis3.API.Models;
using MassTransit;
namespace HealthAxis3.API.Messaging
{
    public class AppointmentBookedConsumer(AppDbContext dbContext, ILogger<AppointmentBookedConsumer> logger) : IConsumer<AppointmentBookedEvent>
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<AppointmentBookedConsumer> _logger = logger;
        public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
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
            _logger.LogInformation($"Received AppointmentBookedEvent: AppointmentId={message.AppointmentId}, PatientName={message.PatientName}, DoctorId={message.DoctorId}, ScheduledDate={message.ScheduledDate}, TimeSlot={message.TimeSlot}");
            await _dbContext.SaveChangesAsync();
        }
    }
}
