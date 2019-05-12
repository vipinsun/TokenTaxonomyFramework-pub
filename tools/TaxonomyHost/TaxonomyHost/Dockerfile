FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
# copy necessary files
COPY CryptletMessages CryptletMessages
COPY CryptletCoreCommon CryptletCoreCommon
COPY CryptletServicesModule CryptletServicesModule
COPY log4net.config .

# Restore all the dependencoes
WORKDIR /app/CryptletMessages
RUN dotnet restore
WORKDIR /app/CryptletCoreCommon
RUN dotnet restore
WORKDIR /app/CryptletServicesModule
RUN dotnet publish -f netcoreapp2.2 -c Release -o out

FROM microsoft/dotnet:2.2-runtime 
LABEL version=cryptletservicesmodule
WORKDIR /app

COPY --from=build /app/CryptletServicesModule/out /app
EXPOSE 11122/tcp
ENTRYPOINT ["dotnet", "Microsoft.Blockchain.Cryptlet.Services.Module.dll"]