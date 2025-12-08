FROM mcr.microsoft.com/dotnet/sdk:10.0 as build
WORKDIR /app

# Kopiuj .csproj i restore
COPY CrudApp.csproj .
RUN dotnet restore CrudApp.csproj

# Kopiuj resztę kodu
COPY . .

# Build i publish
RUN dotnet build CrudApp.csproj -c Release
RUN dotnet publish CrudApp.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "CrudApp.dll"]
