
    $Destination = Join-Path -Path $PSScriptRoot -Child "src/Noobot.Console/Configuration/config.json"
	$Source=Join-Path -Path $PSScriptRoot -Child "src/Noobot.Console/Configuration/config.default.json"
    Write-Output $Destination
    Copy-Item   $Destination
    dotnet restore ".\NoobotTrial.sln" 
	dotnet build ".\NoobotTrial\NoobotTrial.csproj" -c "Release"
