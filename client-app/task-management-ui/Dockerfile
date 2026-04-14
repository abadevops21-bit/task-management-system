# 🔹 Build stage
FROM node:24 AS build

WORKDIR /app

# Copy package files
COPY package*.json ./

RUN npm install

# Copy full source
COPY . .

# Build Angular app
RUN npm run build -- --configuration production


# 🔹 Production stage (Nginx)
FROM nginx:alpine

# Remove default nginx files
RUN rm -rf /usr/share/nginx/html/*

# Copy built app
COPY --from=build /app/dist/task-management-ui /usr/share/nginx/html

# Expose port
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]