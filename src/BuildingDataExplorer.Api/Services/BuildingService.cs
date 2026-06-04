using BuildingDataExplorer.Api.Models;
using BuildingDataExplorer.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace BuildingDataExplorer.Api.Services;

public class BuildingService : IBuildingService
{
    private const string CacheKey = "buildings-all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    private readonly IBuildingRepository _buildingRepository;
    private readonly IMemoryCache _cache;

    public BuildingService(IBuildingRepository buildingRepository, IMemoryCache cache)
    {
        _buildingRepository = buildingRepository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Building>> GetAllBuildingsAsync(CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await _buildingRepository.GetAllAsync(cancellationToken);
        }) ?? [];
    }
}
