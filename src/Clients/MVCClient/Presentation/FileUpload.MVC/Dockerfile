FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY Core/FileUpload.MVC.Application/*.csproj Core/FileUpload.MVC.Application/
COPY Infrastructure/FileUpload.MVC.Infrastructure/*.csproj Infrastructure/FileUpload.MVC.Infrastructure/
COPY Presentation/FileUpload.MVC/*.csproj Presentation/FileUpload.MVC/
RUN dotnet restore Presentation/FileUpload.MVC/*.csproj
COPY . .
RUN dotnet publish Presentation/FileUpload.MVC/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FileUpload.MVC.dll" ]

