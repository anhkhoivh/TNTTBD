FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TNTTBD.Services.sln .
COPY TNTTBD.Services.Api/TNTTBD.Services.Api.csproj TNTTBD.Services.Api/
COPY TNTTBD.Services.Domain/TNTTBD.Services.Domain.csproj TNTTBD.Services.Domain/
COPY TNTTBD.Services.Application/TNTTBD.Services.Application.csproj TNTTBD.Services.Application/
COPY TNTTBD.Services.Infrastructure/TNTTBD.Services.Infrastructure.csproj TNTTBD.Services.Infrastructure/

RUN --mount=type=secret,id=build_ca,target=/usr/local/share/ca-certificates/build-ca.crt \
    if [ -f /usr/local/share/ca-certificates/build-ca.crt ]; then update-ca-certificates; fi \
    && dotnet restore TNTTBD.Services.sln

COPY . .
WORKDIR /src/TNTTBD.Services.Api
RUN dotnet publish -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TNTTBD.Services.Api.dll"]
