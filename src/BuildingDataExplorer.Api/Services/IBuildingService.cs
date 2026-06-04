using BuildingDataExplorer.Api.Models;

namespace BuildingDataExplorer.Api.Services;

public interface IBuildingService
{
    Task<IReadOnlyList<Building>> GetAllBuildingsAsync(CancellationToken cancellationToken = default);
}
