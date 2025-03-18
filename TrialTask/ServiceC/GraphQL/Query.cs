using ServiceC.Entities;
using ServiceC.Repositories;

namespace ServiceC.GraphQL;

public class Query
{
    public async Task<IQueryable<WeatherInfoEntity>> GetWeatherInfo(IWeatherRepository weatherRepository)
    {
        var weatherInfo = await weatherRepository.GetWeatherInfoAsync();
        return weatherInfo.AsQueryable();
    }
}