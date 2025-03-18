using Microsoft.EntityFrameworkCore;
using ServiceC;
using ServiceC.GraphQL;
using ServiceC.Repositories;
using WeatherService = ServiceC.Services.WeatherService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGraphQLServer().AddQueryType<Query>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("WeatherDatabase")));
builder.Services.AddTransient<IWeatherRepository, WeatherRepository>();

var app = builder.Build();

app.MapGrpcService<WeatherService>();
app.MapGraphQL();

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}

app.Run();