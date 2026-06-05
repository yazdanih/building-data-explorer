SET NOCOUNT ON;

DELETE FROM SensorData;
DELETE FROM Rooms;
DELETE FROM Buildings;

INSERT INTO Buildings (Name) VALUES
    (N'Kontorshuset Vasa'),
    (N'Logistikcentrum Hisingen'),
    (N'Bostadshus Eriksberg');

INSERT INTO Rooms (BuildingId, Name)
SELECT b.Id, r.RoomName
FROM (VALUES
    (N'Kontorshuset Vasa', N'Reception'),
    (N'Kontorshuset Vasa', N'Konferens A'),
    (N'Kontorshuset Vasa', N'Konferens B'),
    (N'Kontorshuset Vasa', N'IT-rum'),
    (N'Kontorshuset Vasa', N'Öppet kontorslandskap'),
    (N'Logistikcentrum Hisingen', N'Lastkaj 1'),
    (N'Logistikcentrum Hisingen', N'Lastkaj 2'),
    (N'Logistikcentrum Hisingen', N'Kylrum'),
    (N'Logistikcentrum Hisingen', N'Kontor'),
    (N'Logistikcentrum Hisingen', N'Verkstad'),
    (N'Bostadshus Eriksberg', N'Lägenhet 101'),
    (N'Bostadshus Eriksberg', N'Lägenhet 102'),
    (N'Bostadshus Eriksberg', N'Trapphus'),
    (N'Bostadshus Eriksberg', N'Teknikrum'),
    (N'Bostadshus Eriksberg', N'Arkiv')
) AS r(BuildingName, RoomName)
INNER JOIN Buildings b ON b.Name = r.BuildingName;

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
