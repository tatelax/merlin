FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["websocket-server/websocket-server.csproj", "websocket-server/"]
RUN dotnet restore "websocket-server/websocket-server.csproj"
COPY . .
WORKDIR "/src/websocket-server"
RUN dotnet build "websocket-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "websocket-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "websocket-server.dll"]
