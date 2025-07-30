FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && apt-get install -yq nodejs
RUN node -v
WORKDIR /src
COPY ["UI.Server/UI.Server.csproj", "UI.Server/"]
COPY ["UI.Client/UI.Client.csproj", "UI.Client/"]
COPY ["UI.Shared/UI.Shared.csproj", "UI.Shared/"]
RUN dotnet restore "UI.Server/UI.Server.csproj"
COPY . .
WORKDIR "/src/UI.Server"
RUN dotnet build "./UI.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UI.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN chown app:app /app/ml/
RUN chown -R app:app /app/wwwroot/
RUN mkdir -p /app/ml/data && chown app:app /app/ml/data && chmod 777 /app/ml/data
RUN mkdir -p /app/ml/artifacts && chown app:app /app/ml/artifacts && chmod 777 /app/ml/artifacts
USER $APP_UID
ENV PYTHONIOENCODING=utf-8
ENTRYPOINT ["dotnet", "LaPelicula.UI.Server.dll"]
