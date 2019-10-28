#This is for a Local GitHub cloned instance.
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
# copy necessary files
COPY TaxonomyModel TaxonomyModel
COPY TaxonomyService TaxonomyService

# Restore all the dependencoes
WORKDIR /app/TaxonomyModel
RUN dotnet restore
WORKDIR /app/TaxonomyService
RUN dotnet publish -f netcoreapp2.2 -c Release -o out

FROM microsoft/dotnet:2.2-runtime
LABEL version=ttf1.0
WORKDIR /app

COPY --from=build /app/TaxonomyService/out /app
EXPOSE 8086/tcp
ENTRYPOINT ["dotnet", "TaxonomyService.dll"]