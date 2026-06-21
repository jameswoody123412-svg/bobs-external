param(
    [switch]$Clean = $false,
    [switch]$OpenFolder = $false
)

$projectName = "BobsExternal"
$publishPath = "bin/Release/net8.0-windows/win-x64/publish"
$exePath = "$publishPath/$projectName.exe"

Write-Host "====================================" -ForegroundColor Cyan
Write-Host "Bob's External - Build Script" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan
Write-Host ""

# Clean previous builds if requested
if ($Clean) {
    Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
    Remove-Item -Recurse -Force "bin" -ErrorAction SilentlyContinue
    Remove-Item -Recurse -Force "obj" -ErrorAction SilentlyContinue
    Write-Host "✓ Clean complete" -ForegroundColor Green
    Write-Host ""
}

# Restore dependencies
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Restore failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Restore complete" -ForegroundColor Green
Write-Host ""

# Publish self-contained executable
Write-Host "Publishing self-contained executable..." -ForegroundColor Yellow
Write-Host "This may take a few minutes on first build..." -ForegroundColor Gray
dotnet publish -c Release -r win-x64 --self-contained -p:PublishTrimmed=false
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Publish failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Publish complete" -ForegroundColor Green
Write-Host ""

# Verify exe exists
if (Test-Path $exePath) {
    $fileSize = (Get-Item $exePath).Length / 1MB
    Write-Host "✓ Executable created successfully!" -ForegroundColor Green
    Write-Host "  Location: $exePath" -ForegroundColor Cyan
    Write-Host "  Size: $([math]::Round($fileSize, 2)) MB" -ForegroundColor Cyan
    Write-Host ""
    
    # Open folder if requested
    if ($OpenFolder) {
        Write-Host "Opening publish folder..." -ForegroundColor Yellow
        explorer.exe (Resolve-Path $publishPath)
    }
    
    Write-Host "====================================" -ForegroundColor Green
    Write-Host "Build successful! You can now run:" -ForegroundColor Green
    Write-Host "  $exePath" -ForegroundColor Cyan
    Write-Host "====================================" -ForegroundColor Green
} else {
    Write-Host "✗ Executable not found at expected location!" -ForegroundColor Red
    exit 1
}
