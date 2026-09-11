FROM node:22-bookworm

ENV NODE_ENV=development

WORKDIR /app/frontend

EXPOSE 5173

CMD ["sh", "-c", "npm install && npm run dev -- --host 0.0.0.0 --port 5173"]
