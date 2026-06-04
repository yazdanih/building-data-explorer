using BuildingDataExplorer.Api.Data;
using BuildingDataExplorer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingDataExplorer.Api.Repositories;

public class SensorDataRepository : ISensorDataRepository
{
    private readonly AppDbContext _context;

    public SensorDataRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SensorData>> GetByRoomIdAsync(
        int roomId,
        DateTime? from,
        DateTime? to,
        int? limit,
        CancellationToken cancellationToken = default)
    {
        var query = _context.SensorData
            .Where(s => s.RoomId == roomId);

        if (from.HasValue)
        {
            query = query.Where(s => s.Timestamp >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(s => s.Timestamp <= to.Value);
        }

        query = query.OrderByDescending(s => s.Timestamp);

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<double> GetAverageTemperatureAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return await _context.SensorData
            .Where(s => s.RoomId == roomId)
            .AverageAsync(s => s.Temperature, cancellationToken);
    }

    public async Task<double> GetAverageElectricityAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return await _context.SensorData
            .Where(s => s.RoomId == roomId)
            .AverageAsync(s => s.Electricity, cancellationToken);
    }

    public async Task<IReadOnlyDictionary<int, RoomSensorAverages>> GetAveragesByRoomIdsAsync(
        IReadOnlyList<int> roomIds,
        CancellationToken cancellationToken = default)
    {
        if (roomIds.Count == 0)
        {
            return new Dictionary<int, RoomSensorAverages>();
        }

        return await _context.SensorData
            .Where(s => roomIds.Contains(s.RoomId))
            .GroupBy(s => s.RoomId)
            .Select(g => new
            {
                RoomId = g.Key,
                AvgTemperature = g.Average(x => x.Temperature),
                AvgElectricity = g.Average(x => x.Electricity)
            })
            .ToDictionaryAsync(
                x => x.RoomId,
                x => new RoomSensorAverages(x.AvgTemperature, x.AvgElectricity),
                cancellationToken);
    }

    public async Task<IReadOnlyDictionary<int, RoomSensorAverages>> GetLatestReadingsByRoomIdsAsync(
        IReadOnlyList<int> roomIds,
        CancellationToken cancellationToken = default)
    {
        if (roomIds.Count == 0)
        {
            return new Dictionary<int, RoomSensorAverages>();
        }

        return await _context.SensorData
            .Where(s => roomIds.Contains(s.RoomId))
            .GroupBy(s => s.RoomId)
            .Select(g => new
            {
                RoomId = g.Key,
                Latest = g.OrderByDescending(x => x.Timestamp).First()
            })
            .ToDictionaryAsync(
                x => x.RoomId,
                x => new RoomSensorAverages(x.Latest.Temperature, x.Latest.Electricity),
                cancellationToken);
    }

    public async Task<SensorData> AddAsync(SensorData data, CancellationToken cancellationToken = default)
    {
        _context.SensorData.Add(data);
        await _context.SaveChangesAsync(cancellationToken);
        return data;
    }
}
