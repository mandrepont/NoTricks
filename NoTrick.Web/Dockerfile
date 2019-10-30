FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["NoTrick.Web/NoTrick.Web.csproj", "NoTrick.Web/"]
RUN dotnet restore "NoTrick.Web/NoTrick.Web.csproj"
COPY . .
WORKDIR "/src/NoTrick.Web"
RUN dotnet build "NoTrick.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NoTrick.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoTrick.Web.dll"]