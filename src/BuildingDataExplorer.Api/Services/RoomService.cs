using BuildingDataExplorer.Api.Models.Dtos;
using BuildingDataExplorer.Api.Repositories;

namespace BuildingDataExplorer.Api.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly ISensorDataRepository _sensorDataRepository;

    public RoomService(
        IRoomRepository roomRepository,
        ISensorDataRepository sensorDataRepository)
    {
        _roomRepository = roomRepository;
        _sensorDataRepository = sensorDataRepository;
    }

    public async Task<IReadOnlyList<RoomSummaryDto>> GetRoomsByBuildingIdAsync(
        int buildingId,
        CancellationToken cancellationToken = default)
    {
        var rooms = await _roomRepository.GetByBuildingIdAsync(buildingId, cancellationToken);
        var roomIds = rooms.Select(r => r.Id).ToList();
        var aggregates = await _sensorDataRepository.GetAveragesByRoomIdsAsync(roomIds, cancellationToken);
        var summaries = new List<RoomSummaryDto>();

        foreach (var room in rooms)
        {
            if (!aggregates.TryGetValue(room.Id, out var avg))
            {
                throw new InvalidOperationException("Sequence contains no elements.");
            }

            summaries.Add(new RoomSummaryDto(
                room.Id,
                room.Name,
                avg.Temperature,
                avg.Electricity,
                GetStatus(avg.Temperature)));
        }

        return summaries;
    }

    internal static string GetStatus(double temperature) =>
        temperature switch
        {
            > 26 => "hot",
            < 18 => "cold",
            _ => "normal"
        };
}
