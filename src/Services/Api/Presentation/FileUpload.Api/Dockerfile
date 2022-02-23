FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY Core/FileUpload.Api.Domain/*.csproj Core/FileUpload.Api.Domain/
COPY Core/FileUpload.Api.Application/*.csproj Core/FileUpload.Api.Application/
COPY Infrastructure/FileUpload.Api.Infrastructure/*.csproj Infrastructure/FileUpload.Api.Infrastructure/
COPY Infrastructure/FileUpload.Api.Persistence/*.csproj Infrastructure/FileUpload.Api.Persistence/
COPY Presentation/FileUpload.Api/*.csproj Presentation/FileUpload.Api/
RUN dotnet restore Presentation/FileUpload.Api/*.csproj
COPY . .
RUN dotnet publish Presentation/FileUpload.Api/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.Api.dll" ]

