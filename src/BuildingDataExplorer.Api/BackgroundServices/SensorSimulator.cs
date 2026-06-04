using BuildingDataExplorer.Api.Models.Dtos;
using BuildingDataExplorer.Api.Repositories;
using BuildingDataExplorer.Api.Services;

namespace BuildingDataExplorer.Api.BackgroundServices;

public class SensorSimulator : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SensorSimulator> _logger;
    private readonly IConfiguration _configuration;

    public SensorSimulator(
        IServiceScopeFactory scopeFactory,
        ILogger<SensorSimulator> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_configuration.GetValue("SensorSimulator:Enabled", true))
        {
            _logger.LogInformation("SensorSimulator is disabled");
            return;
        }

        var intervalSeconds = _configuration.GetValue("SensorSimulator:IntervalSeconds", 30);
        _logger.LogInformation("SensorSimulator started (interval: {Interval}s)", intervalSeconds);

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(intervalSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await GenerateReadingsAsync(stoppingToken);
        }
    }

    private async Task GenerateReadingsAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var roomRepository = scope.ServiceProvider.GetRequiredService<IRoomRepository>();
        var sensorService = scope.ServiceProvider.GetRequiredService<ISensorService>();

        var rooms = await roomRepository.GetAllAsync(cancellationToken);
        if (rooms.Count == 0)
        {
            _logger.LogWarning("No rooms found — run seed-data.ps1 first");
            return;
        }

        foreach (var room in rooms)
        {
            var temperature = Random.Shared.NextDouble() * 15 + 15; // 15–30 °C
            var electricity = Random.Shared.NextDouble() * 4.5 + 0.5; // 0.5–5 kWh

            await sensorService.PostSensorDataAsync(
                new PostSensorDataRequest(room.Id, temperature, electricity, null),
                cancellationToken);
        }

        _logger.LogInformation("Generated sensor readings for {Count} rooms", rooms.Count);
    }
}
