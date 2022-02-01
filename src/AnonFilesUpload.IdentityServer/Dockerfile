FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY AnonFilesUpload.IdentityServer/*.csproj AnonFilesUpload.IdentityServer/
COPY AnonFilesUpload.Data/*.csproj AnonFilesUpload.Data/
COPY AnonFilesUpload.Shared/*.csproj AnonFilesUpload.Shared/
RUN dotnet restore AnonFilesUpload.IdentityServer/*.csproj
COPY . .
RUN dotnet publish AnonFilesUpload.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","AnonFilesUpload.IdentityServer.dll" ]