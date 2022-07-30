FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GameTrackerApi/GameTrackerApi.csproj", "GameTrackerApi/"]
RUN dotnet restore "GameTrackerApi/GameTrackerApi.csproj"
COPY . .
WORKDIR "/src/GameTrackerApi"
RUN dotnet build "GameTrackerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameTrackerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameTrackerApi.dll"]
