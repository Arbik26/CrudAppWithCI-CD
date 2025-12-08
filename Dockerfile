FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CrudApp/CrudApp.csproj", "CrudApp/"]
RUN dotnet restore "CrudApp/CrudApp.csproj"
COPY . .
WORKDIR "/src/CrudApp"
RUN dotnet build "CrudApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CrudApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "CrudApp.dll"]
