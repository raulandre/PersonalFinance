FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env

WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
CMD ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://*:8080 dotnet PersonalFinance.dll
