FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM node:10.15-alpine AS client 
WORKDIR /app 
COPY src/Story.Host/ClientApp . 
RUN npm install 
RUN npm run-script build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Story.Host/Story.Host.csproj", "src/Story.Host/"]
RUN dotnet restore "src/Story.Host/Story.Host.csproj"
COPY . .
WORKDIR "/src/src/Story.Host"
RUN dotnet build "Story.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Story.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist /app/dist

ENTRYPOINT ["dotnet", "Story.Host.dll"]