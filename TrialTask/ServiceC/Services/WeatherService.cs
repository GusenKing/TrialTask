using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ServiceC.Entities;
using ServiceC.Repositories;

namespace ServiceC.Services;

public class WeatherService(IWeatherRepository weatherRepository) : ServiceC.WeatherService.WeatherServiceBase
{
    public override async Task<Empty> SetWeather(WeatherInfo request, ServerCallContext context)
    {
        await weatherRepository.AddWeatherInfoAsync(new WeatherInfoEntity
        {
            Temperature = request.Temperature, TemperatureUnit = request.TemperatureUnits,
            Time = DateTime.Parse(request.Time)
        });
        return new Empty();
    }
}