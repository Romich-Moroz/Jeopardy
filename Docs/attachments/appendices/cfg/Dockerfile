#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

EXPOSE 50000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Jeopardy.Web.Matchmaker.Service/Jeopardy.Web.Matchmaker.Service.csproj", "Jeopardy.Web.Matchmaker.Service/"]
COPY ["Jeopardy.Core.Network/Jeopardy.Core.Network.csproj", "Jeopardy.Core.Network/"]
COPY ["Jeopardy.Core.Serialization/Jeopardy.Core.Serialization.csproj", "Jeopardy.Core.Serialization/"]
COPY ["Jeopardy.Core.Data/Jeopardy.Core.Data.csproj", "Jeopardy.Core.Data/"]
COPY ["Jeopardy.Core.Cryptography/Jeopardy.Core.Cryptography.csproj", "Jeopardy.Core.Cryptography/"]
COPY ["Jeopardy.Core.Localization/Jeopardy.Core.Localization.csproj", "Jeopardy.Core.Localization/"]
RUN dotnet restore "Jeopardy.Web.Matchmaker.Service/Jeopardy.Web.Matchmaker.Service.csproj"
COPY . .
WORKDIR "/src/Jeopardy.Web.Matchmaker.Service"
RUN dotnet build "Jeopardy.Web.Matchmaker.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jeopardy.Web.Matchmaker.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jeopardy.Web.Matchmaker.Service.dll"]