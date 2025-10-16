Write-Host "Starting SmartTaskManager with Docker Compose..." -ForegroundColor Green

docker-compose up -d

Write-Host "Containers are starting..." -ForegroundColor Yellow
Write-Host "This may take a minute for SQL Server to initialize..." -ForegroundColor Yellow

Start-Sleep -Seconds 10
Write-Host "Checking services status..." -ForegroundColor Yellow
docker-compose ps

Write-Host "" -ForegroundColor White
Write-Host "Application URLs:" -ForegroundColor Cyan
Write-Host "  - API: http://localhost:8080" -ForegroundColor White
Write-Host "  - Swagger UI: http://localhost:8080" -ForegroundColor White
Write-Host "  - Health Check: http://localhost:8080/api/health" -ForegroundColor White
Write-Host "" -ForegroundColor White
Write-Host "SQL Server:" -ForegroundColor Cyan
Write-Host "  - Server: localhost,1433" -ForegroundColor White
Write-Host "  - Username: sa" -ForegroundColor White
Write-Host "  - Password: YourStrong!Password123" -ForegroundColor White
Write-Host "" -ForegroundColor White
Write-Host "Useful commands:" -ForegroundColor Yellow
Write-Host "  - View logs: docker-compose logs -f" -ForegroundColor Gray
Write-Host "  - Stop: docker-compose down" -ForegroundColor Gray
Write-Host "  - Rebuild: docker-compose build --no-cache" -ForegroundColor Gray