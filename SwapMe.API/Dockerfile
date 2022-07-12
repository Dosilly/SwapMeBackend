FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SwapMe/SwapMe.csproj", "SwapMe/"]
RUN dotnet restore "SwapMe/SwapMe.csproj"
COPY . .
WORKDIR "/src/SwapMe"
RUN dotnet build "SwapMe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SwapMe.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SwapMe.dll"]
