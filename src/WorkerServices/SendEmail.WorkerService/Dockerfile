FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build
WORKDIR /app
COPY src/WorkerServices/SendEmail.WorkerService/*.proj .
RUN dotnet restore *.csproj
COPY . .
RUN dotnet publish src/WorkerServices/SendEmail.WorkerService/*.proj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","SendEmail.WorkerService.dll" ]

