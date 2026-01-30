# Stage 1: Base pour le runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copier les fichiers .csproj et restaurer les dépendances
COPY ["EcommerceAPI.API/EcommerceAPI.API.csproj", "EcommerceAPI.API/"]
COPY ["EcommerceAPI.Application/EcommerceAPI.Application.csproj", "EcommerceAPI.Application/"]
COPY ["EcommerceAPI.Domain/EcommerceAPI.Domain.csproj", "EcommerceAPI.Domain/"]
COPY ["EcommerceAPI.Infrastructure/EcommerceAPI.Infrastructure.csproj", "EcommerceAPI.Infrastructure/"]

RUN dotnet restore "EcommerceAPI.API/EcommerceAPI.API.csproj"

# Copier tout le code source
COPY . .

# Build du projet
WORKDIR "/src/EcommerceAPI.API"
RUN dotnet build "EcommerceAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EcommerceAPI.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EcommerceAPI.API.dll"]
