using BuildingDataExplorer.Api.Models;

namespace BuildingDataExplorer.Api.Repositories;

public interface IRoomRepository
{
    Task<IReadOnlyList<Room>> GetByBuildingIdAsync(int buildingId, CancellationToken cancellationToken = default);
    Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
