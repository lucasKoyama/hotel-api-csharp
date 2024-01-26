FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . ./
# RUN dotnet tool install --global dotnet-ef --version 7.0.4
RUN dotnet restore
# RUN dotnet ef database update
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 5501
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TrybeHotel.dll"]