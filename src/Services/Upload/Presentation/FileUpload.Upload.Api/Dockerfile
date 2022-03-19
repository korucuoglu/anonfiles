FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY Core/FileUpload.Upload.Domain/*.csproj Core/FileUpload.Upload.Domain/
COPY Core/FileUpload.Upload.Application/*.csproj Core/FileUpload.Upload.Application/
COPY Infrastructure/FileUpload.Upload.Infrastructure/*.csproj Infrastructure/FileUpload.Upload.Infrastructure/
COPY Infrastructure/FileUpload.Upload.Persistence/*.csproj Infrastructure/FileUpload.Upload.Persistence/
COPY Presentation/FileUpload.Upload/*.csproj Presentation/FileUpload.Upload/
RUN dotnet restore Presentation/FileUpload.Upload/*.csproj
COPY . .
RUN dotnet publish Presentation/FileUpload.Upload/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.Upload.dll" ]

