using System.Text.Json;
using Common.Dtos.WeatherInfoDto;
using Confluent.Kafka;

namespace ServiceB.Services;

public class KafkaConsumerService
{
    private readonly IConsumer<Null, string> _consumer;

    public KafkaConsumerService()
    {
        var KafkaBootstrapServer =
            Environment.GetEnvironmentVariable("KafkaBootstrapServer", EnvironmentVariableTarget.Process);
        var GroupId =
            Environment.GetEnvironmentVariable("KafkaGroupId", EnvironmentVariableTarget.Process);

        var config = new ConsumerConfig
        {
            BootstrapServers = KafkaBootstrapServer,
            GroupId = GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    private string KafkaBootstrapServer { get; set; }
    private string GroupId { get; set; }

    public void ConsumeMessages()
    {
        var kafkaTopicName =
            Environment.GetEnvironmentVariable("KafkaTopicName", EnvironmentVariableTarget.Process);
        _consumer.Subscribe(kafkaTopicName);
        var grpcClient = new ServiceCGrpcClient();

        try
        {
            while (true)
            {
                var consumeResult = _consumer.Consume();
                Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");

                var deserializedWeatherInfo = JsonSerializer.Deserialize<WeatherInfoDto>(consumeResult.Message.Value);
                if (deserializedWeatherInfo?.Temperature == null)
                    throw new NullReferenceException("Temperature in deserialized weather info is null");

                grpcClient.SetWeather(new WeatherInfo
                {
                    Temperature = (float)deserializedWeatherInfo.Temperature,
                    TemperatureUnits = deserializedWeatherInfo.TemperatureUnit, Time = deserializedWeatherInfo.Time
                });
            }
        }
        catch (ConsumeException e)
        {
            Console.WriteLine($"Error consuming message: {e.Error.Reason}");
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Null reference exception: {e.Message}");
        }
    }
}