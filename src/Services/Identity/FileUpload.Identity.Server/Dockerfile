FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
EXPOSE 80
COPY src/Services/Identity/FileUpload.Identity.Server/*.csproj FileUpload.Identity.Server/
COPY src/Services/Identity/FileUpload.Identity.Data/*.csproj FileUpload.Data/
COPY src/Shared/FileUpload.Shared/*.csproj FileUpload.Shared/
RUN dotnet restore FileUpload.Identity.Server/*.csproj
COPY . .
RUN dotnet publish src/Services/Identity/FileUpload.Identity.Server/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.Identity.Server.dll" ]