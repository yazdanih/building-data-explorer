namespace BuildingDataExplorer.Api.Models;

public class Room
{
    public int Id { get; set; }
    public int BuildingId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Building Building { get; set; } = null!;
    public ICollection<SensorData> SensorReadings { get; set; } = new List<SensorData>();
}
