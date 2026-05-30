#!/bin/bash
set -e

echo ">>> Instalando dependências do frontend..."
cd /app/frontend
npm install
npm run dev -- --host 0.0.0.0 --port 5173 &
FRONTEND_PID=$!

echo ">>> Restaurando pacotes .NET..."
cd /app/backend
dotnet restore

echo ">>> Iniciando .NET com hot reload..."
dotnet watch run \
    --urls "http://0.0.0.0:5091" \
    --no-hot-reload=false &
BACKEND_PID=$!

# Encerra ambos se um deles morrer
wait -n $FRONTEND_PID $BACKEND_PID
kill $FRONTEND_PID $BACKEND_PID 2>/dev/null