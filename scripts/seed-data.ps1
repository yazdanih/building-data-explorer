# Seeds buildings, rooms, and historical sensor readings.
# Room "Arkiv" (Building 3) intentionally has no sensor data (bug demo).
$ErrorActionPreference = "Stop"

$password = "BuildingDataExplorer#2026"
$seedFile = Join-Path $PSScriptRoot "seed-data.sql"
$containerPath = "/tmp/seed-data.sql"

Write-Host "Seeding database..." -ForegroundColor Cyan

docker cp $seedFile "building-data-sql:${containerPath}"
if ($LASTEXITCODE -ne 0) {
    throw "Seed failed. Is the database container running? Run .\scripts\setup-db.ps1 first."
}

$result = docker exec building-data-sql /opt/mssql-tools18/bin/sqlcmd `
    -S localhost -U sa -P $password -C -d BuildingDataExplorer -f 65001 -i $containerPath

if ($LASTEXITCODE -ne 0) {
    throw "Seed failed. Is the database set up? Run .\scripts\setup-db.ps1 first."
}

Write-Host $result
Write-Host "Seed complete (Arkiv has no sensor data - intentional)." -ForegroundColor Green
