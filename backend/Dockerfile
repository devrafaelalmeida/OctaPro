FROM mcr.microsoft.com/dotnet/sdk:10.0

ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_NOLOGO=true \
    NUGET_XMLDOC_MODE=skip \
    ASPNETCORE_ENVIRONMENT=Development

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl unzip procps \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
    && dotnet tool install --global dotnet-ef \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /app/backend

EXPOSE 5091

CMD ["/app/backend/backend/start-debug.sh"]
