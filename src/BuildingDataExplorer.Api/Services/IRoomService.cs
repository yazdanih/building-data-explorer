using BuildingDataExplorer.Api.Models.Dtos;

namespace BuildingDataExplorer.Api.Services;

public interface IRoomService
{
    Task<IReadOnlyList<RoomSummaryDto>> GetRoomsByBuildingIdAsync(int buildingId, CancellationToken cancellationToken = default);
}
