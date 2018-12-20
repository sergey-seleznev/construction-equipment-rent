FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /src

COPY *.csproj .
RUN dotnet restore

COPY / .
RUN dotnet publish -c Release -o /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "ConstructionEquipmentRent.Web.dll"]