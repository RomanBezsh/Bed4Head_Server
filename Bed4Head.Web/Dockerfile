FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Bed4Head.Web/Bed4Head.Web.csproj", "Bed4Head.Web/"]
COPY ["Bed4Head.Application/Bed4Head.Application.csproj", "Bed4Head.Application/"]
COPY ["Bed4Head.Domain/Bed4Head.Domain.csproj", "Bed4Head.Domain/"]
COPY ["Bed4Head.Infrastructure/Bed4Head.Infrastructure.csproj", "Bed4Head.Infrastructure/"]
COPY ["Bed4Head.Common/Bed4Head.Common.csproj", "Bed4Head.Common/"]
RUN dotnet restore "Bed4Head.Web/Bed4Head.Web.csproj"
COPY . .
WORKDIR "/src/Bed4Head.Web"
RUN dotnet build "./Bed4Head.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bed4Head.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bed4Head.Web.dll"]
