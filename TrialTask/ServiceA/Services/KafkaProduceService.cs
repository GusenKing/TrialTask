using Confluent.Kafka;

namespace ServiceA.Services;

public interface IKafkaProducerService
{
    Task SendMessageAsync(string topic, string message);
}

public class KafkaProducerService : IKafkaProducerService
{
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerService(ILogger<KafkaProducerService> logger, IConfiguration configuration)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task SendMessageAsync(string topic, string message)
    {
        try
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            _logger.LogInformation($"Message '{message}' sent to topic '{topic}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending message to Kafka: {ex.Message}");
            throw;
        }
    }
}