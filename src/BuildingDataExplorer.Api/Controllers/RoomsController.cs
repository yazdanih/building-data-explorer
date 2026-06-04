using BuildingDataExplorer.Api.Repositories;
using BuildingDataExplorer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingDataExplorer.Api.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    private readonly ISensorService _sensorService;

    public RoomsController(IRoomRepository roomRepository, ISensorService sensorService)
    {
        _roomRepository = roomRepository;
        _sensorService = sensorService;
    }

    [HttpGet("{id:int}/data")]
    public async Task<IActionResult> GetSensorData(
        int id,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int? limit,
        CancellationToken cancellationToken)
    {
        if (await _roomRepository.GetByIdAsync(id, cancellationToken) is null)
        {
            return NotFound();
        }

        var data = await _sensorService.GetSensorDataAsync(id, from, to, limit, cancellationToken);
        return Ok(data);
    }
}
