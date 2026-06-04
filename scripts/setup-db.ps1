# Starts SQL Server in Docker and applies EF Core migrations.
$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot

Write-Host "Starting SQL Server container..." -ForegroundColor Cyan
Push-Location $Root
try {
    docker compose up -d
}
finally {
    Pop-Location
}

$password = "BuildingDataExplorer#2026"
$maxAttempts = 30

Write-Host "Waiting for SQL Server to accept connections..." -ForegroundColor Cyan
for ($i = 1; $i -le $maxAttempts; $i++) {
    $result = docker exec building-data-sql /opt/mssql-tools18/bin/sqlcmd `
        -S localhost -U sa -P $password -C -Q "SELECT 1" 2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-Host "SQL Server is ready." -ForegroundColor Green
        break
    }

    if ($i -eq $maxAttempts) {
        throw "SQL Server did not become ready within $($maxAttempts * 2) seconds."
    }

    Write-Host "  Attempt $i/$maxAttempts - retrying in 2s..."
    Start-Sleep -Seconds 2
}

Write-Host "Applying EF Core migrations..." -ForegroundColor Cyan
Push-Location $Root
try {
    dotnet ef database update --project src\BuildingDataExplorer.Api
    if ($LASTEXITCODE -ne 0) { throw "Migration failed." }
}
finally {
    Pop-Location
}

Write-Host "Database setup complete." -ForegroundColor Green
