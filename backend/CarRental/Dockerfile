FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarRental/CarRental.csproj", "CarRental/"]
RUN dotnet restore "CarRental/CarRental.csproj"
COPY . .
WORKDIR "/src/CarRental"
RUN dotnet build "CarRental.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarRental.csproj" -c Release -o /app/publish

FROM base AS final
EXPOSE 5000
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRental.dll"]
