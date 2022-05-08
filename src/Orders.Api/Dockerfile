FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Orders.Api/Orders.Api.csproj", "Orders.Api/"]
COPY ["Orders.Command/Orders.Command.csproj", "Orders.Command/"]
COPY ["Orders.Core/Orders.Core.csproj", "Orders.Core/"]
COPY ["Orders.Infrastructure/Orders.Infrastructure.csproj", "Orders.Infrastructure/"]
COPY ["Orders.Query/Orders.Query.csproj", "Orders.Query/"]
RUN dotnet restore "Orders.Api/Orders.Api.csproj"
COPY . .
WORKDIR "/src/Orders.Api"
RUN dotnet build "Orders.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orders.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.Api.dll"]