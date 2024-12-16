# Define base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Define build image using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files and restore dependencies
COPY . .
RUN dotnet restore

# Publish the application to the "out" directory
RUN dotnet publish -c Release -o /app/out

# Define runtime image and copy the published files
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the output from the build stage to the runtime image
COPY --from=build /app/out .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "TravelPlannerAPI.dll"]
