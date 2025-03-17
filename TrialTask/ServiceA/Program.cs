using ServiceA.Services;
using ServiceA.Utility;

var builder = WebApplication.CreateBuilder(args);
await KafkaUtility.CreateTopicAsync(builder.Configuration["Kafka:BootstrapServers"]!,
    builder.Configuration["Kafka:TopicName"]!);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<ApiCallingService>();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddSingleton<PeriodicHostedService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicHostedService>());

var app = builder.Build();

app.Run();