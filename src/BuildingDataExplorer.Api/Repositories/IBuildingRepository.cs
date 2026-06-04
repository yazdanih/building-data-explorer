using BuildingDataExplorer.Api.Models;

namespace BuildingDataExplorer.Api.Repositories;

public interface IBuildingRepository
{
    Task<IReadOnlyList<Building>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Building?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
