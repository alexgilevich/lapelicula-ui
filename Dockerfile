FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && apt-get install -yq nodejs
RUN node -v
WORKDIR /src
COPY ["UI.Server/UI.Server.csproj", "UI.Server/"]
COPY ["UI.Shared/UI.Shared.csproj", "UI.Shared/"]
RUN dotnet restore "UI.Server/UI.Server.csproj"
COPY . .
WORKDIR "/src/UI.Server"
RUN dotnet build "./UI.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UI.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Download Python and create venv in a separate stage at build time to avoid having to download Python at runtime, which requires public access to the internet (via public subnet or NAT gateway) and incurs more costs
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS setup-venv
WORKDIR /app
RUN dotnet tool install --global CSnakes.Stage
ENV PATH="$PATH:/root/.dotnet/tools"
WORKDIR /src
COPY ["UI.Server/", "UI.Server/"]
RUN setup-python --python 3.13 --venv /app/Python/.venv-net --pip-requirements /src/UI.Server/Python/requirements.txt --verbose
RUN /app/Python/.venv-net/bin/python -m pip install --upgrade pip



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=setup-venv /app/Python/.venv-net /app/Python/.venv-net
COPY --from=setup-venv /root/.config/CSnakes/ /home/app/.config/CSnakes/
USER root
RUN chown -R root:root /app
USER $APP_UID
ENV PYTHONIOENCODING=utf-8
ENTRYPOINT ["dotnet", "LaPelicula.UI.Server.dll"]
