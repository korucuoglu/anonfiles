FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY AnonFilesUpload.MVC/*.csproj AnonFilesUpload.MVC/
COPY AnonFilesUpload.Data/*.csproj AnonFilesUpload.Data/
RUN dotnet restore AnonFilesUpload.MVC/*.csproj
COPY . .
RUN dotnet publish AnonFilesUpload.MVC/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","AnonFilesUpload.MVC.dll" ]