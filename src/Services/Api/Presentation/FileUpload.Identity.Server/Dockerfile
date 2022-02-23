FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY Core/FileUpload.Api.Domain/*.csproj Core/FileUpload.Identity.Server.Domain/
COPY Core/FileUpload.Api.Application/*.csproj Core/FileUpload.Identity.Server.Application/
COPY Infrastructure/FileUpload.Api.Persistence/*.csproj Infrastructure/FileUpload.Identity.Server.Persistence/
COPY Presentation/FileUpload.Identity.Server/*.csproj Presentation/FileUpload.Identity.Server/
RUN dotnet restore Presentation/FileUpload.Identity.Server/*.csproj
COPY . .
RUN dotnet publish Presentation/FileUpload.Identity.Server/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.Identity.Server.dll" ]