namespace BuildingDataExplorer.Api.Models.Dtos;

public record RoomSummaryDto(
    int Id,
    string Name,
    double? Temperature,
    double? Electricity,
    string Status);
