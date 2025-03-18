using Microsoft.EntityFrameworkCore;
using ServiceC.Entities;

namespace ServiceC;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WeatherInfoEntity> WeatherInfo { get; set; }
}