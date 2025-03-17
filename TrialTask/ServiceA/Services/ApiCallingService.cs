namespace ServiceA.Services;

public class ApiCallingService(ILogger<ApiCallingService> logger)
{
    private const double KazanLatitude = 55.47;
    private const double KazanLongitude = 49.07;

    private readonly string _currentWeatherApiUrl = "https://api.open-meteo.com/v1/forecast?current=temperature_2m";
    private readonly HttpController _httpController = new();

    public async Task<string?> GetWeatherForecastAsync()
    {
        try
        {
            var response =
                await _httpController.Client.GetAsync(
                    $"{_currentWeatherApiUrl}&latitude={KazanLatitude}&longitude={KazanLongitude}");
            response.EnsureSuccessStatusCode();

            // var weatherForecast = await JsonSerializer.DeserializeAsync<CurrentWeatherDto>(
            //     await response.Content.ReadAsStreamAsync(),
            //     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e.Message);
            return null;
        }
    }
}