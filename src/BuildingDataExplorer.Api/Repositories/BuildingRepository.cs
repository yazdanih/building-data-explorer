using BuildingDataExplorer.Api.Data;
using BuildingDataExplorer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingDataExplorer.Api.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly AppDbContext _context;

    public BuildingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Building>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Buildings
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Building?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Buildings
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }
}
