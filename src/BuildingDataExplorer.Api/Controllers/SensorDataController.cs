using BuildingDataExplorer.Api.Models.Dtos;
using BuildingDataExplorer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingDataExplorer.Api.Controllers;

[ApiController]
[Route("api/sensordata")]
public class SensorDataController : ControllerBase
{
    private readonly ISensorService _sensorService;

    public SensorDataController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpPost]
    public async Task<IActionResult> PostSensorData(
        [FromBody] PostSensorDataRequest request,
        CancellationToken cancellationToken)
    {
        var reading = await _sensorService.PostSensorDataAsync(request, cancellationToken);
        if (reading is null)
        {
            return NotFound(new { message = $"Room {request.RoomId} not found." });
        }

        return Created($"/api/rooms/{reading.RoomId}/data", reading);
    }
}
