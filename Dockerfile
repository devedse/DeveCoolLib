# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /source

# caches restore result by copying csproj file separately
#COPY /NuGet.config /source/
COPY /DeveCoolLib/*.csproj /source/DeveCoolLib/
COPY /DeveCoolLib.ConsoleApp/*.csproj /source/DeveCoolLib.ConsoleApp/
COPY /DeveCoolLib.Tests/*.csproj /source/DeveCoolLib.Tests/
COPY /DeveCoolLib.sln /source/
RUN ls
RUN dotnet restore

# copies the rest of your code
COPY . .
RUN dotnet build --configuration Release
RUN dotnet test --configuration Release ./DeveCoolLib.Tests/DeveCoolLib.Tests.csproj
RUN dotnet publish ./DeveCoolLib.ConsoleApp/DeveCoolLib.ConsoleApp.csproj --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "DeveCoolLib.ConsoleApp.dll"]