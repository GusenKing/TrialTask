using System.Text.Json.Serialization;

namespace ServiceA.Dtos;

public class CurrentWeatherDto
{
    public float Latitude { get; set; }

    public float Longitude { get; set; }

    public float Elevation { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public float GenerationTime { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffset { get; set; }

    public string? Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public string? TimezoneAbbreviation { get; set; }

    [JsonPropertyName("current")] public Current? Current { get; set; }

    [JsonPropertyName("current_units")] public CurrentUnits? CurrentUnits { get; set; }
}

public class Current
{
    public string? Time { get; set; }

    public int? Interval { get; set; }

    [JsonPropertyName("temperature_2m")] public float? Temperature { get; set; }
}

public class CurrentUnits
{
    public string? Time { get; set; }

    public string? Interval { get; set; }

    [JsonPropertyName("temperature_2m")] public string? Temperature { get; set; }
}