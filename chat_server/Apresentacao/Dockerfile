FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Apresentacao/Chat.API.csproj", "Apresentacao/"]
COPY ["Aplicacao/", "Aplicacao/"]
COPY ["Dominio/", "Dominio/"]
COPY ["Infraestrutura/", "Infraestrutura/"]
RUN dotnet restore "./Apresentacao/Chat.API.csproj"
COPY . .
WORKDIR "/src/Apresentacao"
RUN dotnet build "./Chat.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Chat.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.API.dll"]
