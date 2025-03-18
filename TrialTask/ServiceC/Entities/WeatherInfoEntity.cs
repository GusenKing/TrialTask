namespace ServiceC.Entities;

public class WeatherInfoEntity
{
    public long Id { get; set; }

    public float Temperature { get; set; }

    public string TemperatureUnit { get; set; }

    public DateTime Time { get; set; }
}