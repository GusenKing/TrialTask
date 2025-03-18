namespace ServiceA.Services;

public class PeriodicHostedService(
    ILogger<PeriodicHostedService> logger,
    IConfiguration configuration,
    IServiceScopeFactory factory,
    IKafkaProducerService producerService)
    : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(int.Parse(configuration["ApiFetchInterval"]));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
            try
            {
                await using var asyncScope = factory.CreateAsyncScope();
                var apiCallingService = asyncScope.ServiceProvider.GetRequiredService<ApiCallingService>();
                var response = await apiCallingService.GetWeatherForecastAsync();

                if (response != null)
                {
                    await producerService.SendMessageAsync("weather", response);

                    logger.LogInformation(response);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"Failed to execute PeriodicHostedService with exception message {ex.Message}.");
            }
    }
}