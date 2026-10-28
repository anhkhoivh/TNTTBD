# Stage 1: Build Angular app
FROM node:24 AS build
WORKDIR /app
ARG BUILD_CONFIGURATION=production
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build -- --configuration=${BUILD_CONFIGURATION}

# Stage 2: Serve with Nginx
FROM nginx:stable-alpine
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist/TNTTBD.UI/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]