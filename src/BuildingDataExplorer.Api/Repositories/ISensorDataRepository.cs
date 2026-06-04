using BuildingDataExplorer.Api.Models;

namespace BuildingDataExplorer.Api.Repositories;

public interface ISensorDataRepository
{
    Task<IReadOnlyList<SensorData>> GetByRoomIdAsync(
        int roomId,
        DateTime? from,
        DateTime? to,
        int? limit,
        CancellationToken cancellationToken = default);

    Task<double> GetAverageTemperatureAsync(int roomId, CancellationToken cancellationToken = default);

    Task<double> GetAverageElectricityAsync(int roomId, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<int, RoomSensorAverages>> GetAveragesByRoomIdsAsync(
        IReadOnlyList<int> roomIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<int, RoomSensorAverages>> GetLatestReadingsByRoomIdsAsync(
        IReadOnlyList<int> roomIds,
        CancellationToken cancellationToken = default);

    Task<SensorData> AddAsync(SensorData data, CancellationToken cancellationToken = default);
}
