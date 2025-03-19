using Microsoft.EntityFrameworkCore;
using ServiceC.Entities;

namespace ServiceC.Repositories;

public interface IWeatherRepository
{
    public Task AddWeatherInfoAsync(WeatherInfoEntity weatherInfo);

    public Task<List<WeatherInfoEntity>> GetWeatherInfoAsync();
}

public class WeatherRepository(AppDbContext dbContext) : IWeatherRepository
{
    public async Task AddWeatherInfoAsync(WeatherInfoEntity weatherInfo)
    {
        await dbContext.WeatherInfo.AddAsync(weatherInfo);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<WeatherInfoEntity>> GetWeatherInfoAsync()
    {
        return await dbContext.WeatherInfo.AsNoTracking().OrderByDescending(weather => weather.Time).Take(10)
            .ToListAsync();
    }
}