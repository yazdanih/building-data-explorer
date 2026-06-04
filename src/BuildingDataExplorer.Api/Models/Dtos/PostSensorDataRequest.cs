namespace BuildingDataExplorer.Api.Models.Dtos;

public record PostSensorDataRequest(
    int RoomId,
    double Temperature,
    double Electricity,
    DateTime? Timestamp);
