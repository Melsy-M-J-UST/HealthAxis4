namespace HealthAxis3.API.Service.Background
{
    public class HeartbeatBackgroundService : BackgroundService
    {
        private readonly ILogger<HeartbeatBackgroundService> _logger;

        public HeartbeatBackgroundService(
            ILogger<HeartbeatBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Heartbeat running at {Time}", DateTime.Now);
                await Task.Delay( TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
