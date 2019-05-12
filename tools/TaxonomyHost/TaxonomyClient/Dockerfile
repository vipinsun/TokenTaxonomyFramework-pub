# Proper Docker for creating a Cryptlet or Proxy Image to be used.
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
# copy necessary files
COPY TaxonomyClient TaxonomyClient
COPY TaxonomyModel TaxonomyModel

WORKDIR /app/TaxonomyModel
RUN dotnet restore
WORKDIR /app/TaxonomyClient
RUN dotnet publish -f netcoreapp2.2 -c Release -o out

FROM microsoft/dotnet:2.2-runtime 
LABEL version=ttf1.0
WORKDIR /app

COPY --from=build /app/TaxonomyClient/out /app
ENTRYPOINT ["dotnet", "TaxonomyClient.dll"]