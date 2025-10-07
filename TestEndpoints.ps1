$url = "https://drive.google.com/uc?export=download&id=1S55Q7zBzs734gKxKYyEOyFL50fekMvgR"
$outputFile = "Video-Media-Processor.MediaFiles\file_example_MP4_1920_18MG.mp4"

if (Test-Path $outputFile) {
    Write-Host "File already exists: $outputFile"
} else {
    Invoke-WebRequest -Uri $url -OutFile $outputFile
    Write-Host "Downloaded: $outputFile"
}

curl.exe -X Post "https://localhost:7229/api/v1/upload" `
	-F "media=@Video-Media-Processor.MediaFiles\file_example_MP4_1920_18MG.mp4" `
	-F "queries=query1" `
	-F "queries=query2" `
	-F "queries=query3"