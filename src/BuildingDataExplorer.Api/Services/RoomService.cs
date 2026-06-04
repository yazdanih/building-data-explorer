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
        var summaries = new List<RoomSummaryDto>();

        foreach (var room in rooms)
        {
            var avgTemperature = await _sensorDataRepository.GetAverageTemperatureAsync(room.Id, cancellationToken);
            var avgElectricity = await _sensorDataRepository.GetAverageElectricityAsync(room.Id, cancellationToken);

            summaries.Add(new RoomSummaryDto(
                room.Id,
                room.Name,
                avgTemperature,
                avgElectricity,
                GetStatus(avgTemperature)));
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
