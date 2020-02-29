FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
EXPOSE 80

WORKDIR /src

COPY ["EventBus/EventBus/EventBus.csproj", "EventBus/EventBus/"]
COPY ["EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBus/EventBusRabbitMQ/"]
COPY ["EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "EventBus/IntegrationEventLogEF/"]
COPY ["Services/TelegramReceiver/TelegramReceiver.API/TelegramReceiver.API.csproj", "Services/TelegramReceiver/TelegramReceiver.API/"]

RUN dotnet restore Services/TelegramReceiver/TelegramReceiver.API/TelegramReceiver.API.csproj
COPY . .
WORKDIR "Services/TelegramReceiver/TelegramReceiver.API/"

RUN dotnet build -c $BUILD_CONFIGURATION

ENTRYPOINT ["dotnet", "run"]