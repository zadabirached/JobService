FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY JobService.Domain/JobService.Domain.csproj           JobService.Domain/
COPY JobService.Application/JobService.Application.csproj JobService.Application/
COPY JobService.Infrastructure/JobService.Infrastructure.csproj JobService.Infrastructure/
COPY JobService.API/JobService.API.csproj                 JobService.API/
RUN dotnet restore JobService.API/JobService.API.csproj

COPY . .
RUN dotnet publish JobService.API/JobService.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobService.API.dll"]
