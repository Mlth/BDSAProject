Docker setup

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" `
   -p 1433:1433 --name sql1 --hostname sql1 `
   -d `
   mcr.microsoft.com/mssql/server:2022-latest

docker exec -it sql1 "bash"

/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "<YourStrong@Passw0rd>"

CREATE DATABASE GitInsightDB;
GO

Fjern migrations mappen, gå ind i Entities mappen i en terminal, og skriv:
dotnet ef migrations add InitialCreate
dotnet ef database update

Kør programmet

User-secrets

Gå til GitInsight.WebAPI folderen

kør disse kommandoer

dotnet user-secrets init
dotnet user-secrets set "Tokens:gitinsight" "YOUR_TOKEN"
dotnet user-secrets set "ConnectionString" "Server=localhost,1433;Database=GitInsightDB4;User Id=SAPassword=<YourStrong@Passw0rd>Trusted_Connection=False;Encrypt=False"


API links
https://localhost:7024/analysis/Mlth/BDSAProject/author
https://localhost:7024/analysis/Mlth/BDSAProject/frequency