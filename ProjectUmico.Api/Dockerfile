FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProjectUmico.Api/ProjectUmico.Api.csproj", "ProjectUmico.Api/"]
RUN dotnet restore "ProjectUmico.Api/ProjectUmico.Api.csproj"
COPY . .
WORKDIR "/src/ProjectUmico.Api"
RUN dotnet build "ProjectUmico.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectUmico.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectUmico.Api.dll"]
