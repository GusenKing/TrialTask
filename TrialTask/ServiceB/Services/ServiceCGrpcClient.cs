using Grpc.Net.Client;

namespace ServiceB.Services;

public class ServiceCGrpcClient
{
    public void SetWeather(WeatherInfo weather)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7249");
        var client = new WeatherService.WeatherServiceClient(channel);

        var reply = client.SetWeather(weather);
    }
}