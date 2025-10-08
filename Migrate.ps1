# Get the original path
$originalPath = Get-Location

# Check if folder argument is provided
if ($args.Count -eq 0) {
    Write-Host "Error: Please provide a folder name"
    exit 1
}

$folderName = $args[0]

# Check if folder exists
if (-not (Test-Path $folderName)) {
    Write-Host "Error: Folder '$folderName' does not exist"
    exit 1
}

# Change to the specified folder
Set-Location $folderName

try {
    # Create evolution folder in original path if it doesn't exist
    $evolutionPath = Join-Path $originalPath "Video-Media-Processor.DB\Evolution"
    if (-not (Test-Path $evolutionPath)) {
        New-Item -ItemType Directory -Path $evolutionPath | Out-Null
    }

    # Run Entity Framework migrations
    dotnet ef migrations add "Migration_$(Get-Date -Format 'yyyyMMdd_HHmmss')" 2>> "$originalPath\ef_errors.log"
    
    if ($LASTEXITCODE -eq 0) {
        # Get the last applied migration
		$lastMigration = dotnet ef migrations list --no-build | Where-Object { $_ -match "^[0-9]" } | Select-Object -Last 1

		# Generate script from last migration to current
		dotnet ef migrations script $lastMigration --output "$evolutionPath\migration_$(Get-Date -Format 'yyyyMMdd_HHmmss').sql" 2>> "$originalPath\ef_errors.log"
    }
}
catch {
    $_.Exception.Message | Out-File "$originalPath\Video-Media-Processor.BuildLogs\ef_errors.log" -Append
}
finally {
    # Return to original path
    Set-Location $originalPath
}