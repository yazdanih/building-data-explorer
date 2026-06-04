# Generates bulk sensor data for performance testing (default: 50 000 rows).
param(
    [int]$Count = 50000
)

$ErrorActionPreference = "Stop"

$password = "BuildingDataExplorer#2026"

Write-Host "Generating $Count sensor readings..." -ForegroundColor Cyan

$loadSql = @"
SET NOCOUNT ON;

DECLARE @target INT = $Count;
DECLARE @batch INT = 5000;
DECLARE @inserted INT = 0;

WHILE @inserted < @target
BEGIN
    DECLARE @batchSize INT = CASE WHEN @target - @inserted < @batch THEN @target - @inserted ELSE @batch END;

    INSERT INTO SensorData (RoomId, Temperature, Electricity, Timestamp)
    SELECT TOP (@batchSize)
        r.Id,
        15.0 + (ABS(CHECKSUM(NEWID())) % 150) / 10.0,
        0.5 + (ABS(CHECKSUM(NEWID())) % 450) / 100.0,
        DATEADD(SECOND, -(ABS(CHECKSUM(NEWID())) % 864000), SYSUTCDATETIME())
    FROM Rooms r
    CROSS JOIN (SELECT TOP (@batchSize) 1 AS n FROM sys.all_objects a CROSS JOIN sys.all_objects b) nums;

    SET @inserted = @inserted + @batchSize;
    PRINT CONCAT('Inserted ', @inserted, ' / ', @target);
END;

SELECT COUNT(*) AS TotalSensorReadings FROM SensorData;
"@

$result = docker exec building-data-sql /opt/mssql-tools18/bin/sqlcmd `
    -S localhost -U sa -P $password -C -d BuildingDataExplorer -Q $loadSql

if ($LASTEXITCODE -ne 0) {
    throw "Load generation failed. Run setup-db.ps1 and seed-data.ps1 first."
}

Write-Host $result
Write-Host "Load generation complete." -ForegroundColor Green
