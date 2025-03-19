using Grpc.Net.Client;

namespace ServiceB.Services;

public class ServiceCGrpcClient
{
    public ServiceCGrpcClient()
    {
        ChannelAddress = Environment.GetEnvironmentVariable("GrpcChannelAddress", EnvironmentVariableTarget.Process);
    }

    private string ChannelAddress { get; }

    public void SetWeather(WeatherInfo weather)
    {
        using var channel =
            GrpcChannel.ForAddress(ChannelAddress);
        var client = new WeatherService.WeatherServiceClient(channel);

        var reply = client.SetWeather(weather);
    }
}