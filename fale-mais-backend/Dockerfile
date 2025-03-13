FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY FaleMais.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish FaleMais.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/out ./

COPY --from=build /app/appsettings.json ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "FaleMais.dll"]
