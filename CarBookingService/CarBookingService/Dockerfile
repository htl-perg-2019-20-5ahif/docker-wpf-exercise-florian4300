#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["CarBookingService/CarBookingService.csproj", "CarBookingService/"]
RUN dotnet restore "CarBookingService/CarBookingService.csproj"
COPY . .
WORKDIR "/src/CarBookingService"
RUN dotnet build "CarBookingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarBookingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarBookingService.dll"]