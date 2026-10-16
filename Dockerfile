# Estágio 1: restaura e publica a API com o SDK completo
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia só os .csproj primeiro para aproveitar o cache do restore
COPY CreditFlow.Domain/CreditFlow.Domain.csproj CreditFlow.Domain/
COPY CreditFlow.Application/CreditFlow.Application.csproj CreditFlow.Application/
COPY CreditFlow.Infrastructure/CreditFlow.Infrastructure.csproj CreditFlow.Infrastructure/
COPY CreditFlow.API/CreditFlow.API.csproj CreditFlow.API/
RUN dotnet restore CreditFlow.API/CreditFlow.API.csproj

COPY CreditFlow.Domain/ CreditFlow.Domain/
COPY CreditFlow.Application/ CreditFlow.Application/
COPY CreditFlow.Infrastructure/ CreditFlow.Infrastructure/
COPY CreditFlow.API/ CreditFlow.API/
RUN dotnet publish CreditFlow.API/CreditFlow.API.csproj -c Release -o /app/publish --no-restore

# Estágio 2: imagem final só com o runtime do ASP.NET, rodando sem root
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "CreditFlow.API.dll"]
