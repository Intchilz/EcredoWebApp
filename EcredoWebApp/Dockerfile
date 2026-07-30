FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file
COPY EcredoWebApp.csproj ./

# Restore dependencies
RUN dotnet restore EcredoWebApp.csproj

# Copy the rest of the project
COPY . .

# Publish
RUN dotnet publish EcredoWebApp.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "EcredoWebApp.dll"]