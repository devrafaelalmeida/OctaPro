#!/usr/bin/env bash
set -euo pipefail

dotnet build OctaPro.csproj \
  --configuration Debug \
  /property:DebugType=portable \
  "/consoleloggerparameters:NoSummary;ForceNoAlign"

exec dotnet /app/backend/bin/Debug/net10.0/OctaPro.dll --urls http://0.0.0.0:5091
