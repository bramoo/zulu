FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
RUN apt-get update -y && apt-get install -y gnupg2 && curl -sL https://deb.nodesource.com/setup_11.x | bash - && apt-get update -y && apt-get install -y nodejs
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8192
EXPOSE 8192

FROM microsoft/dotnet:2.2-sdk-stretch AS build
RUN apt-get update -y && apt-get install -y gnupg2 && curl -sL https://deb.nodesource.com/setup_11.x | bash - && apt-get update -y && apt-get install -y build-essential nodejs
WORKDIR /src
COPY zulu.csproj .
RUN dotnet restore zulu.csproj
COPY . .
WORKDIR /src
RUN dotnet build zulu.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish zulu.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "zulu.dll"]
