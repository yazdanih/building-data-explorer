# Builds and runs the API (includes SensorSimulator background service).
$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot

Push-Location $Root
try {
    Write-Host "Building API..." -ForegroundColor Cyan
    dotnet build src\BuildingDataExplorer.Api
    if ($LASTEXITCODE -ne 0) { throw "Build failed." }

    Write-Host "Starting API at http://localhost:5053 (Ctrl+C to stop)..." -ForegroundColor Cyan
    dotnet run --project src\BuildingDataExplorer.Api --launch-profile http --no-build
}
finally {
    Pop-Location
}
