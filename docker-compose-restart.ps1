Write-Host "Restarting SmartTaskManager services..." -ForegroundColor Yellow
docker-compose restart
Write-Host "Services restarted!" -ForegroundColor Green