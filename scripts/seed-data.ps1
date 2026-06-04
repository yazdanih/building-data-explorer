# Seeds buildings, rooms, and historical sensor readings.
# Room "Arkiv" (Building 3) intentionally has no sensor data (bug demo).
$ErrorActionPreference = "Stop"

$password = "BuildingDataExplorer#2026"

Write-Host "Seeding database..." -ForegroundColor Cyan

$seedSql = @"
SET NOCOUNT ON;

DELETE FROM SensorData;
DELETE FROM Rooms;
DELETE FROM Buildings;
DBCC CHECKIDENT ('SensorData', RESEED, 0);
DBCC CHECKIDENT ('Rooms', RESEED, 0);
DBCC CHECKIDENT ('Buildings', RESEED, 0);

INSERT INTO Buildings (Name) VALUES
    (N'Kontorshuset Vasa'),
    (N'Logistikcentrum Hisingen'),
    (N'Bostadshus Eriksberg');

INSERT INTO Rooms (BuildingId, Name) VALUES
    (1, N'Reception'),
    (1, N'Konferens A'),
    (1, N'Konferens B'),
    (1, N'IT-rum'),
    (1, N'Öppet kontorslandskap'),
    (2, N'Lastkaj 1'),
    (2, N'Lastkaj 2'),
    (2, N'Kylrum'),
    (2, N'Kontor'),
    (2, N'Verkstad'),
    (3, N'Lägenhet 101'),
    (3, N'Lägenhet 102'),
    (3, N'Trapphus'),
    (3, N'Teknikrum'),
    (3, N'Arkiv');

DECLARE @hours INT = 24;
DECLARE @h INT = @hours;

WHILE @h >= 1
BEGIN
    INSERT INTO SensorData (RoomId, Temperature, Electricity, Timestamp)
    SELECT
        r.Id,
        18.0 + (ABS(CHECKSUM(NEWID())) % 120) / 10.0,
        0.5 + (ABS(CHECKSUM(NEWID())) % 450) / 100.0,
        DATEADD(HOUR, -@h, SYSUTCDATETIME())
    FROM Rooms r
    WHERE r.Name <> N'Arkiv';

    SET @h = @h - 1;
END;

SELECT
    (SELECT COUNT(*) FROM Buildings) AS Buildings,
    (SELECT COUNT(*) FROM Rooms) AS Rooms,
    (SELECT COUNT(*) FROM SensorData) AS SensorReadings;
"@

$result = docker exec building-data-sql /opt/mssql-tools18/bin/sqlcmd `
    -S localhost -U sa -P $password -C -d BuildingDataExplorer -Q $seedSql

if ($LASTEXITCODE -ne 0) {
    throw "Seed failed. Is the database set up? Run .\scripts\setup-db.ps1 first."
}

Write-Host $result
Write-Host "Seed complete (Arkiv has no sensor data — intentional)." -ForegroundColor Green
