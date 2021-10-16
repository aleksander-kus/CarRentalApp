FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln ./
COPY CarRental/*.csproj ./CarRental/
COPY CarRental.Domain/*.csproj ./CarRental.Domain/
COPY CarRental.Infrastructure/*.csproj ./CarRental.Infrastructure/
COPY CarRental.Domain.Test/*.csproj ./CarRental.Domain.Test/
COPY CarRental.Infrastructure.Test/*.csproj ./CarRental.Infrastructure.Test/

RUN dotnet restore
COPY . .
WORKDIR /src/CarRental
RUN dotnet build -c Release -o /app

WORKDIR /src/CarRental.Domain
RUN dotnet build -c Release -o /app

WORKDIR /src/CarRental.Domain.Test
RUN dotnet build -c Release -o /app

WORKDIR /src/CarRental.Infrastructure
RUN dotnet build -c Release -o /app

WORKDIR /src/CarRental.Infrastructure.Test
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
EXPOSE 5000
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CarRental.dll"]