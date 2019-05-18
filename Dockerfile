# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /source

# caches restore result by copying csproj file separately
#COPY /NuGet.config /source/
COPY /DeveCoolLib/*.csproj /source/DeveCoolLib/
COPY /DeveCoolLib.Tests/*.csproj /source/DeveCoolLib.Tests/
COPY /DeveCoolLib.sln /source/
RUN ls
RUN dotnet restore

# copies the rest of your code
COPY . .
RUN dotnet build --configuration Release
RUN dotnet test --configuration Release ./DeveCoolLib.Tests/DeveCoolLib.Tests.csproj