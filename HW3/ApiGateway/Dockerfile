FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./EDomainLib/EDomainLib.csproj ./EDomainLib/
COPY ./ApiGateway/ApiGateway.csproj ./ApiGateway/

RUN dotnet restore ./ApiGateway/ApiGateway.csproj

COPY . .

WORKDIR /src/ApiGateway
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
