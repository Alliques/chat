# Stage 1: Build
FROM node:18 AS build
WORKDIR /app
COPY Clients/package.json .
RUN npm install
COPY /Clients /app
RUN npm run build

# Stage 2: Runtime
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]