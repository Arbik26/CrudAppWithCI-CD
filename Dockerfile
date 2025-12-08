FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /app

# Kopiuj pliki projektu
COPY CrudApp/CrudApp.csproj ./CrudApp/
COPY . .

# Restore i build
WORKDIR /app/CrudApp
RUN dotnet restore
RUN dotnet build -c Release

# Publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=0 /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "CrudApp.dll"]
