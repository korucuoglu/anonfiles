FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80

COPY src/Shared/FileUpload.Shared/*.csproj Shared/FileUpload.Shared/

COPY src/Clients/MVCClient/Core/FileUpload.MVC.Application/*.csproj Core/FileUpload.MVC.Application/
COPY src/Clients/MVCClient/Infrastructure/FileUpload.MVC.Infrastructure/*.csproj Infrastructure/FileUpload.MVC.Infrastructure/
COPY src/Clients/MVCClient/Presentation/FileUpload.MVC/*.csproj Presentation/FileUpload.MVC/
RUN dotnet restore Presentation/FileUpload.MVC/*.csproj
COPY . .
RUN dotnet publish src/Clients/MVCClient/Presentation/FileUpload.MVC/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.MVC.dll" ]

