
$dockerPath = "C:\Program Files\Docker\Docker\Docker Desktop.exe"
#$SqlPassword = "passwordV$03"
$guid = [guid]::NewGuid()

 function Handle-Docker()
 {
 if((get-process "docker" -ea SilentlyContinue) -eq $Null){ 
        echo "Docker not running. Starting docker.."
        Start-Process -FilePath $dockerPath
        Read-Host -Prompt "Wait docker to initialise press any key to continue..."
        'docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=passwordV$03" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest' | cmd
}

else{ 
    echo "Docker already running."
    Read-Host -Prompt "Wait docker to initialise press any key to continue..."
    'docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=passwordV$03" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest' | cmd
 }
 }

 function Sql-Dotnet()
 {
    dotnet ef migrations add ${guid} --context ApplicationDbContext -p ./ProjectUmico.Infrastructure/ -s ./ProjectUmico.Api/ -o Persistance/Migrations
    dotnet ef database update --context ApplicationDbContext -p ./ProjectUmico.Infrastructure -s ./ProjectUmico.Api

 }

 function SqlLite-Dotnet()
 {
    #Write-Output "dotnet ef migrations add ${guid} --context SqlLiteDbContext -p ./ProjectUmico.Infrastructure/ -s ./ProjectUmico.Api/ -o Persistance/Migrations/SqlLite"
    #dotnet build
    dotnet ef migrations add ${guid} --context SqlLiteDbContext -p ./ProjectUmico.Infrastructure/ -s ./ProjectUmico.Api/ -o Persistance/Migrations/SqlLite
    dotnet ef database update --context SqlLiteDbContext -p ./ProjectUmico.Infrastructure -s ./ProjectUmico.Api --no-build
 }


$env = $args[0]



if($env -eq "prod")
{
    Handle-Docker
    SQL-Dotnet
}
if( ($env -eq "dev") -or ($env -eq "") )
{
   SqlLite-Dotnet
}















 
