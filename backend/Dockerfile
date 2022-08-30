FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Aihr.Calculator.Api/Aihr.Calculator.Api.csproj", "Aihr.Calculator.Api/"]
COPY ["Aihr.Calculator.Common/Aihr.Calculator.Common.csproj", "Aihr.Calculator.Common/"]
RUN dotnet restore "Aihr.Calculator.Api/Aihr.Calculator.Api.csproj"
COPY . .
WORKDIR "/src/Aihr.Calculator.Api"
RUN dotnet build "Aihr.Calculator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aihr.Calculator.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aihr.Calculator.Api.dll"]
