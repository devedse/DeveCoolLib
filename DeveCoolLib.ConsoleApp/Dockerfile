#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DeveCoolLib.ConsoleApp/DeveCoolLib.ConsoleApp.csproj", "DeveCoolLib.ConsoleApp/"]
COPY ["DeveCoolLib/DeveCoolLib.csproj", "DeveCoolLib/"]
RUN dotnet restore "DeveCoolLib.ConsoleApp/DeveCoolLib.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/DeveCoolLib.ConsoleApp"
RUN dotnet build "DeveCoolLib.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeveCoolLib.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeveCoolLib.ConsoleApp.dll"]