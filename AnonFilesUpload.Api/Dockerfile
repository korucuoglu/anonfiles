FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY AnonFilesUpload.Api/*.csproj AnonFilesUpload.Api/
COPY AnonFilesUpload.Data/*.csproj AnonFilesUpload.Data/
RUN dotnet restore AnonFilesUpload.Api/*.csproj
COPY . .
RUN dotnet publish AnonFilesUpload.Api/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","AnonFilesUpload.Api.dll" ]