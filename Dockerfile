FROM node:16 AS build-frontend

WORKDIR /app

COPY ./client/package.json ./  
COPY ./client/package-lock.json ./  
COPY ./client .  

RUN npm run build  

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-backend

WORKDIR /app

COPY Messenger/Messenger.csproj ./Messenger/
RUN dotnet restore ./Messenger/Messenger.csproj
COPY Messenger/ ./Messenger/

RUN dotnet publish ./Messenger/Messenger.csproj -c Release -o /app/out

FROM nginx:alpine AS production-frontend

COPY --from=build-frontend /app/build /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production-backend

WORKDIR /app

COPY --from=build-backend /app/out ./

ENV ASPNETCORE_URLS=http://0.0.0.0:5218

ENTRYPOINT ["dotnet", "Messenger.dll"]
