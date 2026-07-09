namespace HealthAxis3.API.Events
{
    using HealthAxis3.API.Data;
    using HealthAxis3.API.Models;
    using MassTransit;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;
    using System.Text.Json;

    public class AppointmentBookedConsumer(AppDbContext dbContext, Logger<AppointmentBookedConsumer> logger, IConfiguration config) : BackgroundService, IConsumer<AppointmentBookedEvent>
    {
        private readonly ILogger<AppointmentBookedConsumer> _logger = logger;
        private IConnection? _connection;
        private IChannel? _channel;

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            var rabbitConfig = config.GetSection("RabbitMQ");
            var factory = new ConnectionFactory()
            {
                HostName = rabbitConfig["HostName"]!,
                Port = int.Parse(rabbitConfig["port"]!),
                UserName = rabbitConfig["UserName"]!,
                Password = rabbitConfig["Password"]!,
                VirtualHost = rabbitConfig["VirtualHost"]!
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            _logger.LogInformation("Hosting service started in the background-------------------");
            await base.StartAsync(cancellationToken);
        }
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
            dbContext.Notifications.Add(notification);
            await dbContext.SaveChangesAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_channel is null)
            {
                throw new InvalidOperationException("RabbitMQ channel is not initialized.");
            }
            var queueName = config.GetSection("RabbitMQ")["QueueName"]!;
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken
            );
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var appEvent = JsonSerializer.Deserialize<AppointmentBookedEvent>(message);
                _logger.LogInformation(appEvent?.EventType);
                _logger.LogInformation("Event:{AppointmentId}, {PatientName}, {TimeSlot}, {ScheduledDate}",
                appEvent?.AppointmentId,
                appEvent?.PatientName,
                appEvent?.TimeSlot,
                appEvent?.ScheduledDate
            );
                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            };
            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping hosting service...");
            if(_channel!=null) await _channel.CloseAsync();
            if(_connection != null) await _connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
