using BuildingDataExplorer.Api.Repositories;
using BuildingDataExplorer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingDataExplorer.Api.Controllers;

[ApiController]
[Route("api/buildings")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _buildingService;
    private readonly IBuildingRepository _buildingRepository;
    private readonly IRoomService _roomService;

    public BuildingsController(
        IBuildingService buildingService,
        IBuildingRepository buildingRepository,
        IRoomService roomService)
    {
        
        _buildingService = buildingService;
        _buildingRepository = buildingRepository;
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBuildings(CancellationToken cancellationToken)
    {
        var buildings = await _buildingService.GetAllBuildingsAsync(cancellationToken);
        return Ok(buildings);
    }

    [HttpGet("{id:int}/rooms")]
    public async Task<IActionResult> GetRooms(int id, CancellationToken cancellationToken)
    {
        if (await _buildingRepository.GetByIdAsync(id, cancellationToken) is null)
        {
            return NotFound();
        }

        var rooms = await _roomService.GetRoomsByBuildingIdAsync(id, cancellationToken);
        return Ok(rooms);
    }
}
