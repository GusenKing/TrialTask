using System.Text.Json;
using Common.Dtos.WeatherInfoDto;
using Confluent.Kafka;

namespace ServiceB.Services;

public class KafkaConsumerService
{
    private readonly IConsumer<Null, string> _consumer;

    public KafkaConsumerService(string bootstrapServer)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServer,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    public void ConsumeMessages(string topic)
    {
        _consumer.Subscribe(topic);
        var grpcClient = new ServiceCGrpcClient();

        try
        {
            while (true)
            {
                var consumeResult = _consumer.Consume();
                var deserializedWeatherInfo = JsonSerializer.Deserialize<WeatherInfoDto>(consumeResult.Message.Value);

                if (deserializedWeatherInfo?.Temperature == null)
                    throw new NullReferenceException("Temperature in deserialized weather info is null");

                grpcClient.SetWeather(new WeatherInfo
                {
                    Temperature = (float)deserializedWeatherInfo.Temperature,
                    TemperatureUnits = deserializedWeatherInfo.TemperatureUnit, Time = deserializedWeatherInfo.Time
                });

                Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
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