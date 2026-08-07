FROM mcr.microsoft.com/dotnet/sdk:10.0

ENV DOTNET_USE_POLLING_FILE_WATCHER=1 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_NOLOGO=true \
    NUGET_XMLDOC_MODE=skip \
    ASPNETCORE_ENVIRONMENT=Development \
    NODE_VERSION=22.x

# Node.js via NodeSource (mais limpo que tar manual)
RUN apt-get update && apt-get install -y curl build-essential \
    && curl -fsSL https://deb.nodesource.com/setup_${NODE_VERSION} | bash - \
    && apt-get install -y nodejs \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# vsdbg — debugger remoto do .NET para VS Code
RUN curl -sSL https://aka.ms/getvsdbgsh \
    | bash /dev/stdin -v latest -l /vsdbg

RUN dotnet tool install --global dotnet-ef

ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /app

# Expõe: .NET API | React dev server | (porta reservada para vsdbg via docker exec, não precisa expor)
EXPOSE 5091 4200

COPY start.sh /start.sh
RUN chmod +x /start.sh

CMD ["/start.sh"]