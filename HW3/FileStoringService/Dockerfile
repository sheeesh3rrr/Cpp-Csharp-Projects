FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./EDomainLib/EDomainLib.csproj ./EDomainLib/
COPY ./FileStoringService/FileStoringService.csproj ./FileStoringService/

RUN dotnet restore ./FileStoringService/FileStoringService.csproj

COPY . .

WORKDIR /src/FileStoringService
RUN dotnet publish "FileStoringService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FileStoringService.dll"]
