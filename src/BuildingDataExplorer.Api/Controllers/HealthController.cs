using BuildingDataExplorer.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuildingDataExplorer.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;

    public HealthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                return StatusCode(503, new { status = "unhealthy", database = "unreachable" });
            }

            return Ok(new { status = "healthy", database = "connected" });
        }
        catch (Exception)
        {
            return StatusCode(503, new { status = "unhealthy", database = "error" });
        }
    }
}
