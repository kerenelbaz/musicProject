# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official ASP.NET Core SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["kilyrics-serverSide/kilyrics-serverSide.csproj", "kilyrics-serverSide/"]
RUN dotnet restore "kilyrics-serverSide/kilyrics-serverSide.csproj"
COPY . .
WORKDIR "/src/kilyrics-serverSide"
RUN dotnet build "kilyrics-serverSide.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kilyrics-serverSide.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kilyrics-serverSide.dll"]

