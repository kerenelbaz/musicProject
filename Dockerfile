FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY kilyrics-serverSide/ ./kilyrics-serverSide/
RUN dotnet restore "kilyrics-serverSide/kilyrics-serverSide.csproj"
RUN dotnet build "kilyrics-serverSide/kilyrics-serverSide.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kilyrics-serverSide/kilyrics-serverSide.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kilyrics-serverSide.dll"]
