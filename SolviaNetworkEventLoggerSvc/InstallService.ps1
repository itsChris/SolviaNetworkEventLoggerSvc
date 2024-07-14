$serviceName = "NetworkEventLoggerService"
$binaryPath = "C:\Users\ChristianCasutt\source\repos\SolviaNetworkEventLoggerSvc\SolviaNetworkEventLoggerSvc\bin\Debug\net8.0\SolviaNetworkEventLoggerSvc.exe"

New-Service -Name $serviceName -BinaryPathName $binaryPath -DisplayName $serviceName -Description "A service to log network events and ping results" -StartupType Automatic
Start-Service -Name $serviceName
