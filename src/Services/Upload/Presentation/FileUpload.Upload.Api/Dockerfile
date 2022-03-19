FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80

COPY src/Shared/FileUpload.Shared/*.csproj Shared/FileUpload.Shared/

COPY src/Services/Upload/Core/FileUpload.Upload.Domain/*.csproj Core/FileUpload.Upload.Domain/
COPY src/Services/Upload/Core/FileUpload.Upload.Application/*.csproj Core/FileUpload.Upload.Application/
COPY src/Services/Upload/Infrastructure/FileUpload.Upload.Infrastructure/*.csproj Infrastructure/FileUpload.Upload.Infrastructure/
COPY src/Services/Upload/Infrastructure/FileUpload.Upload.Persistence/*.csproj Infrastructure/FileUpload.Upload.Persistence/
COPY src/Services/Upload/Presentation/FileUpload.Upload.Api/*.csproj Presentation/FileUpload.Upload.Api/
RUN dotnet restore Presentation/FileUpload.Upload.Api/*.csproj
COPY . .
RUN dotnet publish src/Services/Upload/Presentation/FileUpload.Upload.Api/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.Upload.Api.dll" ]

