# 🔹 Build Stage
FROM --platform=linux/amd64 node:20-alpine AS build

WORKDIR /app

COPY package*.json ./

# ✅ use install (not ci)
RUN npm install --no-audit --no-fund

COPY . .

# build (SSR already fixed earlier)
RUN npm run build


# 🔹 Production Stage
FROM --platform=linux/amd64 nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/dist/task-management-ui /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]