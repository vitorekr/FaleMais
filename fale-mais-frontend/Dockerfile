# Etapa 1: Construção do projeto Angular
FROM node:22.13.0 as build

WORKDIR /app

# Copiar arquivos necessários
COPY package*.json ./

RUN npm install --legacy-peer-deps

COPY . .

RUN npm run build --configuration=production

FROM nginx:latest

RUN rm /etc/nginx/conf.d/default.conf

COPY nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist/fale-mais-frontend/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
