#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-amd64 AS build
WORKDIR /src
COPY ["DeveCoolLib.ConsoleApp/DeveCoolLib.ConsoleApp.csproj", "DeveCoolLib.ConsoleApp/"]
COPY ["DeveCoolLib/DeveCoolLib.csproj", "DeveCoolLib/"]
RUN dotnet restore "DeveCoolLib.ConsoleApp/DeveCoolLib.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/DeveCoolLib.ConsoleApp"
RUN dotnet build "DeveCoolLib.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_VERSION
ARG VER=${BUILD_VERSION:-1.0.0}
RUN dotnet publish "DeveCoolLib.ConsoleApp.csproj" -c Release -o /app/publish /p:Version=$VER /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeveCoolLib.ConsoleApp.dll"]