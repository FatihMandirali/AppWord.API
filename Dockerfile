#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AppWord.API/AppWord.API.csproj", "AppWord.API/"]
COPY ["AppWord.Core/AppWord.Core.csproj", "AppWord.Core/"]
COPY ["AppWord.Data/AppWord.Data.csproj", "AppWord.Data/"]
COPY ["AppWord.Model/AppWord.Model.csproj", "AppWord.Model/"]
RUN dotnet restore "AppWord.API/AppWord.API.csproj"
COPY . .
WORKDIR "/src/AppWord.API"
RUN dotnet build "AppWord.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppWord.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppWord.API.dll"]
