FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY SmartRegistry.Web/SmartRegistry.Web.csproj SmartRegistry.Web/
RUN dotnet restore SmartRegistry.Web/SmartRegistry.Web.csproj
COPY . .
WORKDIR /src/SmartRegistry.Web
RUN dotnet build SmartRegistry.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SmartRegistry.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SmartRegistry.Web.dll"]
