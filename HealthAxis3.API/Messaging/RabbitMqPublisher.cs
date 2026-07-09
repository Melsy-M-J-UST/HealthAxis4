using HealthAxis3.API.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HealthAxis3.API.Messaging
{
    public class RabbitMqPublisher : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private string _queueName = string.Empty;
        public RabbitMqPublisher(IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory()
            {
                HostName = rabbitConfig["HostName"]!,
                Port= int.Parse(rabbitConfig["port"]!),
                UserName = rabbitConfig["UserName"]!,
                Password = rabbitConfig["Password"]!,
                VirtualHost = rabbitConfig["VirtualHost"]!
            };
            _queueName=rabbitConfig["QueueName"]!;
            _connection=factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
            _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete:false,
                arguments:null
                ).GetAwaiter().GetResult();
        }
        public async Task PublishAsync(AppointmentBookedEvent appointmentBookedEvent)
        {
            var message=JsonSerializer.Serialize(appointmentBookedEvent);
            var body=Encoding.UTF8.GetBytes(message);
            var properties= new BasicProperties { Persistent = true };
            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _queueName,
                mandatory: false,
                basicProperties: properties,
                body: body
                );
        }
        public void Dispose()
        {
            _channel.CloseAsync();
            _connection.CloseAsync();
        }
    }
}
