# Serves the TypeScript frontend on http://localhost:5173 (requires API on :5053).
$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot
$Frontend = Join-Path $Root "frontend"

Push-Location $Frontend
try {
    if (-not (Test-Path "dist\app.js")) {
        Write-Host "Compiling TypeScript..." -ForegroundColor Cyan
        npx --yes -p typescript tsc -p tsconfig.json
    }

    Write-Host "Frontend at http://localhost:5173 (API must run on :5053)" -ForegroundColor Cyan
    npx --yes serve -l 5173 .
}
finally {
    Pop-Location
}
