FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY FileUpload.Api/*.csproj FileUpload.Api/
COPY FileUpload.Data/*.csproj FileUpload.Data/
COPY FileUpload.Shared/*.csproj FileUpload.Shared/
RUN dotnet restore FileUpload.Api/*.csproj
COPY . .
RUN dotnet publish FileUpload.Api/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.Api.dll" ]