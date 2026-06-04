using BuildingDataExplorer.Api.Models;
using BuildingDataExplorer.Api.Models.Dtos;
using BuildingDataExplorer.Api.Repositories;

namespace BuildingDataExplorer.Api.Services;

public class SensorService : ISensorService
{
    private readonly IRoomRepository _roomRepository;
    private readonly ISensorDataRepository _sensorDataRepository;

    public SensorService(IRoomRepository roomRepository, ISensorDataRepository sensorDataRepository)
    {
        _roomRepository = roomRepository;
        _sensorDataRepository = sensorDataRepository;
    }

    public async Task<IReadOnlyList<SensorData>> GetSensorDataAsync(
        int roomId,
        DateTime? from,
        DateTime? to,
        int? limit,
        CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (room is null)
        {
            return [];
        }

        return await _sensorDataRepository.GetByRoomIdAsync(roomId, from, to, limit, cancellationToken);
    }

    public async Task<SensorData?> PostSensorDataAsync(
        PostSensorDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room is null)
        {
            return null;
        }

        var reading = new SensorData
        {
            RoomId = request.RoomId,
            Temperature = request.Temperature,
            Electricity = request.Electricity,
            Timestamp = request.Timestamp ?? DateTime.UtcNow
        };

        return await _sensorDataRepository.AddAsync(reading, cancellationToken);
    }
}
