using BuildingDataExplorer.Api.Data;
using BuildingDataExplorer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingDataExplorer.Api.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Rooms
            .OrderBy(r => r.BuildingId)
            .ThenBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Room>> GetByBuildingIdAsync(int buildingId, CancellationToken cancellationToken = default)
    {
        return await _context.Rooms
            .Where(r => r.BuildingId == buildingId)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
