namespace BuildingDataExplorer.Api.Models;

public class SensorData
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public double Temperature { get; set; }
    public double Electricity { get; set; }
    public DateTime Timestamp { get; set; }

    public Room Room { get; set; } = null!;
}
