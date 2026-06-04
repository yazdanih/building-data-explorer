using BuildingDataExplorer.Api.Models;
using BuildingDataExplorer.Api.Models.Dtos;

namespace BuildingDataExplorer.Api.Services;

public interface ISensorService
{
    Task<IReadOnlyList<SensorData>> GetSensorDataAsync(
        int roomId,
        DateTime? from,
        DateTime? to,
        int? limit,
        CancellationToken cancellationToken = default);

    Task<SensorData?> PostSensorDataAsync(PostSensorDataRequest request, CancellationToken cancellationToken = default);
}
